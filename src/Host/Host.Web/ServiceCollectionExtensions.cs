using System;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mkh.Cache.Redis;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Core;
using Mkh.Excel.Abstractions;
using Mkh.Excel.Core;
using Mkh.Excel.EPPlus;
using Mkh.Host.Web.BackgroundServices;
using Mkh.Host.Web.Filters;
using Mkh.Host.Web.Swagger.Conventions;
using Mkh.Module.Abstractions;
using Mkh.Module.Web;
using Mkh.Utils.Json;
using Mkh.Utils.Json.Converters;

namespace Mkh.Host.Web;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加MVC功能
    /// </summary>
    /// <param name="services"></param>
    /// <param name="modules"></param>
    /// <param name="hostOptions"></param>
    /// <param name="env"></param>
    /// <returns></returns>
    public static IMvcBuilder AddMvc(this IServiceCollection services, IModuleCollection modules, Options.HostOptions hostOptions, IHostEnvironment env)
    {
        //添加多语言支持
        services.AddLocalization(opt => opt.ResourcesPath = "Resources");

        var mvcBuilder = services.AddMvc(c =>
            {
                if (hostOptions!.Swagger || !env.IsProduction())
                {
                    //API分组约定
                    c.Conventions.Add(new ApiExplorerGroupConvention());
                }

                //审计日志全局过滤器
                c.Filters.Add(typeof(AuditFilterAttribute));
            })
            .AddJsonOptions(options =>
            {
                //不区分大小写的反序列化
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                //属性名称使用 camel 大小写
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                //最大限度减少字符转义
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                //添加日期转换器
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                //添加多态嵌套序列化
                options.JsonSerializerOptions.AddPolymorphism();

            })
            //多语言
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var module = modules.FirstOrDefault(m => m.LayerAssemblies.Core == type.Assembly);
                    if (module != null && module.LocalizerType != null)
                    {
                        return factory.Create(module.LocalizerType);
                    }

                    return factory.Create(type);
                };
            });

        //添加模块
        mvcBuilder.AddModules(modules);

        return mvcBuilder;
    }

    /// <summary>
    /// 添加CORS
    /// </summary>
    /// <param name="services"></param>
    /// <param name="hostOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddCors(this IServiceCollection services, Options.HostOptions hostOptions)
    {
        services.AddCors(options =>
        {
            /*浏览器的同源策略，就是出于安全考虑，浏览器会限制从脚本发起的跨域HTTP请求（比如异步请求GET, POST, PUT, DELETE, OPTIONS等等，
                    所以浏览器会向所请求的服务器发起两次请求，第一次是浏览器使用OPTIONS方法发起一个预检请求，第二次才是真正的异步请求，
                    第一次的预检请求获知服务器是否允许该跨域请求：如果允许，才发起第二次真实的请求；如果不允许，则拦截第二次请求。
                    Access-Control-Max-Age用来指定本次预检请求的有效期，单位为秒，，在此期间不用发出另一条预检请求。*/
            var preflightMaxAge = hostOptions.PreflightMaxAge > 0 ? new TimeSpan(0, 0, hostOptions.PreflightMaxAge) : new TimeSpan(0, 30, 0);

            options.AddPolicy("Default",
                builder => builder.SetIsOriginAllowed(_ => true)
                    .SetPreflightMaxAge(preflightMaxAge)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition"));//下载文件时，文件名称会保存在headers的Content-Disposition属性里面
        });

        return services;
    }

    /// <summary>
    /// 添加数据库
    /// </summary>
    /// <param name="services"></param>
    /// <param name="modules"></param>
    /// <returns></returns>
    public static IServiceCollection AddData(this IServiceCollection services, IModuleCollection modules)
    {
        foreach (var module in modules)
        {
            var dbOptions = module.Options!.Db;
            var dbContextType = module.LayerAssemblies.Core.GetTypes().FirstOrDefault(m => typeof(DbContext).IsAssignableFrom(m));

            var dbBuilder = services.AddMkhDb(dbContextType, opt =>
            {
                opt.Provider = dbOptions.Provider;

                //Sqlite数据库自动创建数据库文件
                if (dbOptions.ConnectionString.IsNull() && dbOptions.Provider == DbProvider.Sqlite)
                {
                    string dbFilePath = Path.Combine(AppContext.BaseDirectory, "db");
                    if (!Directory.Exists(dbFilePath))
                    {
                        Directory.CreateDirectory(dbFilePath);
                    }

                    dbOptions.ConnectionString = $"Data Source={dbFilePath}/{module.Code}.db;Mode=ReadWriteCreate";
                }

                opt.ConnectionString = dbOptions.ConnectionString;
                opt.Log = dbOptions.Log;
                opt.TableNamePrefix = dbOptions.TableNamePrefix;
                opt.TableNameSeparator = dbOptions.TableNameSeparator;
                opt.Version = dbOptions.Version;
                opt.ModuleCode = module.Code;
            });

            //加载仓储
            dbBuilder.AddRepositoriesFromAssembly(module.LayerAssemblies.Core);

            //启用代码优先
            if (dbOptions.CodeFirst)
            {
                dbBuilder.AddCodeFirst(opt =>
                {
                    opt.CreateDatabase = dbOptions.CreateDatabase;
                    opt.UpdateColumn = dbOptions.UpdateColumn;
                    opt.InitData = dbOptions.InitData;
                    opt.InitDataFilePath = module.DbInitFilePath;
                });
            }

            //特性事务
            foreach (var dic in module.ApplicationServices)
            {
                dbBuilder.AddTransactionAttribute(dic.Key, dic.Value);
            }

            dbBuilder.Build();
        }

        return services;
    }

    /// <summary>
    /// 添加后台服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        //添加创建表后台服务
        services.AddHostedService<CreateTableBackgroundService>();

        return services;
    }

    /// <summary>
    /// 添加缓存
    /// </summary>
    /// <param name="services"></param>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration cfg)
    {
        var builder = services.AddCache();

        var provider = cfg["Mkh:Cache:Provider"];

        if (provider != null && provider.ToLower() == "redis")
        {
            builder.UseRedis(cfg);
        }

        builder.Build();

        return services;
    }

    /// <summary>
    /// 添加Excel
    /// </summary>
    /// <param name="services"></param>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static IServiceCollection AddExcel(this IServiceCollection services, IConfiguration cfg)
    {
        services.Configure<ExcelOptions>(cfg.GetSection("Mkh:Excel"));

        services.AddExcel(builder =>
        {
            var options = services.BuildServiceProvider().GetService<IOptions<ExcelOptions>>().Value;
            if (options.Provider.IsNull() || options.Provider.Equals("epplus", StringComparison.OrdinalIgnoreCase))
            {
                builder.UseEPPlus();
            }
        });

        return services;
    }
}