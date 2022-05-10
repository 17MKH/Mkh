using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Logger;
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Abstractions.Schema;
using Mkh.Data.Abstractions.Sharding;
using Mkh.Data.Core.Descriptors;
using Mkh.Data.Core.Sharding;
using Mkh.Data.Core.Sharding.Providers;

namespace Mkh.Data.Core;

internal class DbBuilder : IDbBuilder
{
    private readonly IList<Assembly> _repositoryAssemblies = new List<Assembly>();
    private readonly List<Action> _actions = new();
    private readonly Type _dbContextType;

    public IServiceCollection Services { get; set; }

    public DbOptions Options { get; set; }

    public CodeFirstOptions CodeFirstOptions { get; set; }

    public IDbContext DbContext { get; set; }

    public DbBuilder(IServiceCollection services, DbOptions options, Type dbContextType)
    {
        Services = services;
        Options = options;
        _dbContextType = dbContextType;
    }

    public IDbBuilder AddRepositoriesFromAssembly(Assembly assembly)
    {
        if (assembly == null)
            return this;

        _repositoryAssemblies.Add(assembly);
        return this;
    }

    public IDbBuilder AddAction(Action action)
    {
        if (action == null)
            return this;

        _actions.Add(action);
        return this;
    }

    public void Build()
    {
        if (Options.Provider != DbProvider.Sqlite)
            Check.NotNull(Options.ConnectionString, "连接字符串未配置");

        //创建数据库上下文
        CreateDbContext();

        //加载仓储
        LoadRepositories();

        //执行自定义委托
        foreach (var action in _actions)
        {
            action.Invoke();
        }
    }

    #region ==私有方法==

    /// <summary>
    /// 创建数据库上下文
    /// </summary>
    private void CreateDbContext()
    {
        var sp = Services.BuildServiceProvider();
        var dbLogger = new DbLogger(Options, sp.GetService<IDbLoggerProvider>());
        var accountResolver = sp.GetService<IOperatorResolver>();

        //获取数据库适配器的程序集
        var dbAdapterAssemblyName = Assembly.GetCallingAssembly().GetName().Name!.Replace("Core", "Adapter.") + Options.Provider;
        var dbAdapterAssembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(dbAdapterAssemblyName));

        //创建数据库上下文实例，通过反射设置属性
        DbContext = (IDbContext)Activator.CreateInstance(_dbContextType);
        _dbContextType.GetProperty("Options")?.SetValue(DbContext, Options);
        _dbContextType.GetProperty("Logger")?.SetValue(DbContext, dbLogger);
        _dbContextType.GetProperty("Adapter")?.SetValue(DbContext, CreateDbAdapter(dbAdapterAssemblyName, dbAdapterAssembly));
        _dbContextType.GetProperty("SchemaProvider")?.SetValue(DbContext, CreateSchemaProvider(dbAdapterAssemblyName, dbAdapterAssembly));
        _dbContextType.GetProperty("CodeFirstProvider")?.SetValue(DbContext, CreateCodeFirstProvider(dbAdapterAssemblyName, dbAdapterAssembly, Services));
        _dbContextType.GetProperty("AccountResolver")?.SetValue(DbContext, accountResolver);

        // ReSharper disable once AssignNullToNotNullAttribute
        Services.AddSingleton(_dbContextType, DbContext);
        Services.AddSingleton(typeof(IDbContext), DbContext);
    }

    /// <summary>
    /// 创建数据库适配器
    /// </summary>
    /// <returns></returns>
    private IDbAdapter CreateDbAdapter(string dbAdapterAssemblyName, Assembly dbAdapterAssembly)
    {
        var dbAdapterType = dbAdapterAssembly.GetType($"{dbAdapterAssemblyName}.{Options.Provider}DbAdapter");

        Check.NotNull(dbAdapterType, $"数据库适配器{dbAdapterAssemblyName}未安装");

        var dbAdapter = (IDbAdapter)Activator.CreateInstance(dbAdapterType!);
        dbAdapterType.GetProperty("Options")!.SetValue(dbAdapter, Options);
        return dbAdapter;
    }

    /// <summary>
    /// 创建数据库架构提供器实例
    /// </summary>
    /// <returns></returns>
    private ISchemaProvider CreateSchemaProvider(string dbAdapterAssemblyName, Assembly dbAdapterAssembly)
    {
        var schemaProviderType = dbAdapterAssembly.GetType($"{dbAdapterAssemblyName}.{Options.Provider}SchemaProvider");

        return (ISchemaProvider)Activator.CreateInstance(schemaProviderType!, Options.ConnectionString);
    }

    /// <summary>
    /// 创建数据库代码优先提供器实例
    /// </summary>
    /// <returns></returns>
    private ICodeFirstProvider CreateCodeFirstProvider(string dbAdapterAssemblyName, Assembly dbAdapterAssembly, IServiceCollection services)
    {
        var schemaProviderType = dbAdapterAssembly.GetType($"{dbAdapterAssemblyName}.{Options.Provider}CodeFirstProvider");
        return (ICodeFirstProvider)Activator.CreateInstance(schemaProviderType!, CodeFirstOptions, DbContext, services);
    }

    /// <summary>
    /// 加载仓储
    /// </summary>
    private void LoadRepositories()
    {
        if (_repositoryAssemblies.IsNullOrEmpty())
            return;

        foreach (var assembly in _repositoryAssemblies)
        {
            /*
             * 仓储约定：
             * 1、仓储统一放在Repositories目录中
             * 2、仓储默认使用SqlServer数据库，如果数据库之间有差异无法通过ORM规避时，采用以下方式解决：
             *    a）将对应的方法定义为虚函数
             *    b）假如当前方法在MySql中实现有差异，则在Repositories新建一个MySql目录
             *    c）在MySql目录中新建一个仓储（我称之为兼容仓储）并继承默认仓储
             *    d）在新建的兼容仓储中使用MySql语法重写对应的方法
             */

            var repositoryTypes = assembly.GetTypes()
                .Where(m => !m.IsInterface && typeof(IRepository).IsImplementType(m))
                //排除兼容仓储
                .Where(m => m.FullName!.Split('.')[^2].EqualsIgnoreCase("Repositories"))
                .ToList();

            //兼容仓储列表
            var compatibilityRepositoryTypes = assembly.GetTypes()
                .Where(m => !m.IsInterface && typeof(IRepository).IsImplementType(m))
                //根据数据库类型来过滤
                .Where(m => m.FullName!.Split('.')[^2].EqualsIgnoreCase(Options.Provider.ToString()))
                .ToList();

            foreach (var type in repositoryTypes)
            {
                //按照框架约定，仓储的第三个接口类型就是所需的仓储接口
                var interfaceType = type.GetInterfaces()[2];

                //按照约定，仓储接口的第一个接口的泛型参数即为对应实体类型
                var entityType = interfaceType.GetInterfaces()[0].GetGenericArguments()[0];

                var entityDescriptor = new EntityDescriptor(DbContext, entityType);

                //设置分表
                if (entityDescriptor.IsSharding)
                {
                    switch (entityDescriptor.ShardingPolicy)
                    {
                        case ShardingPolicy.Year:
                            entityDescriptor.ShardingPolicyProvider = new YearShardingPolicyProvider();
                            break;
                        case ShardingPolicy.Quarter:
                            entityDescriptor.ShardingPolicyProvider = new QuarterShardingPolicyProvider();
                            break;
                        case ShardingPolicy.Month:
                            entityDescriptor.ShardingPolicyProvider = new MonthShardingPolicyProvider();
                            break;
                        case ShardingPolicy.Day:
                            entityDescriptor.ShardingPolicyProvider = new DayShardingPolicyProvider();
                            break;
                        case ShardingPolicy.Custom:
                            if (entityDescriptor.CustomShardingPolicyProviderType != null)
                            {
                                entityDescriptor.ShardingPolicyProvider = (IShardingPolicyProvider)Services.BuildServiceProvider().GetService(entityDescriptor.CustomShardingPolicyProviderType);
                            }
                            break;
                    }
                }

                //保存实体描述符
                DbContext.EntityDescriptors.Add(entityDescriptor);

                //优先使用兼容仓储
                var implementationType = compatibilityRepositoryTypes.FirstOrDefault(m => m.Name == type.Name) ?? type;

                Services.AddScoped(interfaceType, sp =>
                {
                    var instance = Activator.CreateInstance(implementationType);
                    var initMethod = implementationType.GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic);
                    initMethod!.Invoke(instance, new Object[] { DbContext, sp });

                    //保存仓储实例
                    var manager = sp.GetService<IRepositoryManager>();
                    manager?.Add((IRepository)instance);

                    return instance;
                });

                //保存仓储描述符
                DbContext.RepositoryDescriptors.Add(new RepositoryDescriptor(entityType, interfaceType, implementationType));
            }
        }
    }

    #endregion
}