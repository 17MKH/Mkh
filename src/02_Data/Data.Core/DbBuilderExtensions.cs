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
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Core.Internal;
using Mkh.Utils.Helpers;

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

        //加载初始化数据对象
        var initData = LoadInitData(options);

        options.AfterCreateTable = (dbContext, entityDescriptor) =>
        {
            //初始化表数据
            InitTableData(initData, dbContext, entityDescriptor, builder.Services);
        };

        builder.CodeFirstOptions = options;

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
            var jsonHelper = new JsonHelper();

            if (initData.All(m => m.Name != entityDescriptor.Name))
                return;

            var property = initData.FirstOrDefault(m => m.Name == entityDescriptor.Name);

            var list = (IList)jsonHelper.Deserialize(property.Value.ToString(), typeof(List<>).MakeGenericType(entityDescriptor.EntityType));

            if (list == null || list.Count == 0)
                return;

            var repositoryDescriptor = dbContext.RepositoryDescriptors.FirstOrDefault(m => m.EntityType == entityDescriptor.EntityType);

            var repository = (IRepository)services.BuildServiceProvider().GetService(repositoryDescriptor!.InterfaceType);

            using var uow = dbContext.NewUnitOfWork();
            repository.BindingUow(uow);

            var tasks = new List<Task>();
            foreach (var item in list)
            {
                tasks.Add(repository.Add(item));
            }

            Task.WaitAll(tasks.ToArray());

            uow.SaveChanges();
        }
    }
}