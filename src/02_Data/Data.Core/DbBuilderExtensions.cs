using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Events;
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Core.Internal;
using Mkh.Utils.Json;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DbBuilderExtensions
{
    /// <summary>
    /// 使用数据库
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="connectionString"></param>
    /// <param name="provider"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IDbBuilder UseDb(this IDbBuilder builder, string connectionString, DbProvider provider, Action<DbOptions> configure = null)
    {
        builder.Options.ConnectionString = connectionString;
        builder.Options.Provider = provider;
        configure?.Invoke(builder.Options);

        return builder;
    }

    /// <summary>
    /// 添加CodeFirst功能
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure">自定义配置</param>
    /// <returns></returns>
    public static IDbBuilder AddCodeFirst(this IDbBuilder builder, Action<CodeFirstOptions> configure = null)
    {
        var options = new CodeFirstOptions();
        configure?.Invoke(options);

        builder.CodeFirstOptions = options;

        builder.RegisterDatabaseEvents();

        builder.RegisterTableEvents();

        builder.AddAction(() =>
        {
            //优先使用自定义的代码优先提供器，毕竟默认的建库见表语句并不能满足所有人的需求
            var provider = options.CustomCodeFirstProvider ?? builder.DbContext.CodeFirstProvider;

            if (provider != null)
            {
                //先有库
                provider.CreateDatabase();

                //后有表
                provider.CreateTable();
            }
        });

        return builder;
    }

    /// <summary>
    /// 添加事务特性功能
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="serviceType"></param>
    /// <param name="implementationType"></param>
    /// <returns></returns>
    public static IDbBuilder AddTransactionAttribute(this IDbBuilder builder, Type serviceType, Type implementationType)
    {
        var services = builder.Services;

        //尝试添加代理生成器
        services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();

        //添加需要特性事务的服务
        services.AddScoped(serviceType, sp =>
        {
            var target = sp.GetService(implementationType);
            var generator = sp.GetService<IProxyGenerator>();
            var manager = sp.GetService<IRepositoryManager>();
            var interceptor = new TransactionInterceptor(builder.DbContext, manager);
            var proxy = generator!.CreateInterfaceProxyWithTarget(serviceType, target, interceptor);
            return proxy;
        });

        return builder;
    }

    /// <summary>
    /// 加载初始化数据
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    private static List<JsonProperty> LoadInitData(CodeFirstOptions options)
    {
        //初始化数据
        if (options.InitData && options.InitDataFilePath.NotNull() && File.Exists(options.InitDataFilePath))
        {
            using var jsonReader = new StreamReader(options.InitDataFilePath, Encoding.UTF8);
            var str = jsonReader.ReadToEnd();

            var doc = JsonDocument.Parse(str);
            return doc.RootElement.EnumerateObject().ToList();
        }

        return new List<JsonProperty>();
    }

    private static void InitTableData(List<JsonProperty> initData, IDbContext dbContext, IEntityDescriptor entityDescriptor, IServiceCollection services)
    {
        //初始化数据
        if (initData.Any())
        {
            if (initData.All(m => m.Name != entityDescriptor.Name))
                return;

            var property = initData.FirstOrDefault(m => m.Name == entityDescriptor.Name);

            var list = (IList)JsonHelper.Instance.Deserialize(property.Value.ToString(), typeof(List<>).MakeGenericType(entityDescriptor.EntityType));

            if (list.Count == 0)
                return;

            var repositoryDescriptor = dbContext.RepositoryDescriptors.FirstOrDefault(m => m.EntityType == entityDescriptor.EntityType);

            var repository = services.BuildServiceProvider().GetService(repositoryDescriptor!.InterfaceType);
            var repositoryType = repository.GetType();

            using var uow = dbContext.NewUnitOfWork();

            var bindingUowMethod = repositoryType.GetMethod("BindingUow");
            bindingUowMethod!.Invoke(repository, new object[] { uow });

            var bulkAddMethod = repositoryType.GetMethods().Single(m => m.Name == "BulkAdd" && m.GetParameters().Length == 3);
            var bulkAddTask = (Task)bulkAddMethod!.Invoke(repository, new object[] { list, 0, uow });
            bulkAddTask!.Wait();

            uow.SaveChanges();
        }
    }

    /// <summary>
    /// 注册数据库相关事件
    /// </summary>
    /// <param name="builder"></param>
    private static void RegisterDatabaseEvents(this IDbBuilder builder)
    {
        var databaseCreateEvents = builder.Services.BuildServiceProvider().GetServices<IDatabaseCreateEvent>();

        builder.CodeFirstOptions.BeforeCreateDatabase = dbContext =>
        {
            var eventContext = new DatabaseCreateContext
            {
                DbContext = dbContext,
                CreateTime = DateTime.Now
            };

            foreach (var databaseCreateEvent in databaseCreateEvents)
            {
                databaseCreateEvent.OnBeforeCreate(eventContext);
            }
        };

        builder.CodeFirstOptions.AfterCreateDatabase = dbContext =>
        {
            var eventContext = new DatabaseCreateContext
            {
                DbContext = dbContext,
                CreateTime = DateTime.Now
            };

            foreach (var databaseCreateEvent in databaseCreateEvents)
            {
                databaseCreateEvent.OnAfterCreate(eventContext);
            }
        };
    }

    /// <summary>
    /// 注册表创建事件
    /// </summary>
    /// <param name="builder"></param>
    private static void RegisterTableEvents(this IDbBuilder builder)
    {
        //加载初始化数据对象
        var initData = LoadInitData(builder.CodeFirstOptions);

        var tableCreateEvents = builder.Services.BuildServiceProvider().GetServices<ITableCreateEvent>();

        builder.CodeFirstOptions.BeforeCreateTable = (dbContext, entityDescriptor) =>
        {
            var tableCreateContext = new TableCreateContext
            {
                DbContext = dbContext,
                EntityDescriptor = entityDescriptor,
                CreateTime = DateTime.Now
            };

            foreach (var tableCreateEvent in tableCreateEvents)
            {
                tableCreateEvent.OnBeforeCreate(tableCreateContext);
            }
        };

        builder.CodeFirstOptions.AfterCreateTable = (dbContext, entityDescriptor) =>
        {
            //初始化表数据
            InitTableData(initData, dbContext, entityDescriptor, builder.Services);

            var tableCreateContext = new TableCreateContext
            {
                DbContext = dbContext,
                EntityDescriptor = entityDescriptor,
                CreateTime = DateTime.Now
            };

            foreach (var tableCreateEvent in tableCreateEvents)
            {
                tableCreateEvent.OnAfterCreate(tableCreateContext);
            }
        };

    }
}