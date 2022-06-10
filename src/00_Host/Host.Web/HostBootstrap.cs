using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mkh.Config.Core;
using Mkh.Excel.Core;
using Mkh.Host.Web.Middleware;
using Mkh.Host.Web.Swagger;
using Mkh.Logging.Core;
using Mkh.Module.Abstractions;
using Mkh.Module.Core;
using Mkh.Module.Web;
using Mkh.Utils;
using Serilog;
using HostOptions = Mkh.Host.Web.Options.HostOptions;

namespace Mkh.Host.Web;

/// <summary>
/// 宿主启动器
/// </summary>
public class HostBootstrap
{
    /// <summary>
    /// 启动应用
    /// </summary>
    /// <returns></returns>
    public void Run(string[] args)
    {
        var options = LoadOptions();

        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseDefaultServiceProvider(opt => { opt.ValidateOnBuild = false; });

        //使用Serilog日志
        builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom
                .Configuration(hostingContext.Configuration)
                .Enrich
                .FromLogContext();
        });

        //绑定URL
        builder.WebHost.UseUrls(options.Urls);

        var services = builder.Services;

        var env = builder.Environment;
        var cfg = builder.Configuration;

        var modules = ConfigureServices(services, env, cfg, options);

        var app = builder.Build();

        Configure(app, modules, options, env);

        app.Run();
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    private IModuleCollection ConfigureServices(IServiceCollection services, IWebHostEnvironment env, IConfiguration cfg, HostOptions options)
    {
        services.AddSingleton(options);

        //注入Utils
        services.AddUtils(cfg);

        //添加模块
        var modules = services.AddModulesCore(env, cfg);

        //添加模块的前置服务
        services.AddModulePreServices(modules, env, cfg);

        //添加Swagger
        services.AddSwagger(modules, options, env);

        //添加缓存
        services.AddCache(cfg);

        //添加对象映射
        services.AddMappers(modules);

        //添加MVC配置
        services.AddMvc(modules, options, env);

        //添加CORS
        services.AddCors(options);

        //解决Multipart body length limit 134217728 exceeded
        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = int.MaxValue;
        });

        //添加HttpClient相关
        services.AddHttpClient();

        //添加日志功能
        services.AddMkhLogging();

        //添加配置功能
        services.AddConfig(cfg, modules);

        //添加模块的自定义特有的服务
        services.AddModuleServices(modules, env, cfg);

        //添加身份认证和授权
        services.AddMkhAuth(cfg).UseJwt();

        //添加数据库
        services.AddData(modules);

        //添加后台服务
        services.AddBackgroundServices();

        //excel导出配置
        services.AddExcel(cfg);

        //添加模块的后置服务
        services.AddModulePostServices(modules, env, cfg);

        return modules;
    }

    /// <summary>
    /// 配置中间件
    /// </summary>
    private void Configure(WebApplication app, IModuleCollection modules, HostOptions options, IWebHostEnvironment env)
    {
        //使用全局异常处理中间件
        app.UseMiddleware<ExceptionHandleMiddleware>();

        //基地址
        app.UsePathBase(options);

        //开放目录
        if (options.OpenDirs != null && options.OpenDirs.Any())
        {
            //设置默认页
            app.UseDefaultPage(options);

            options.OpenDirs.ForEach(m =>
            {
                app.OpenDir(m, env);
            });
        }

        //反向代理
        if (options!.Proxy)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }

        //路由
        app.UseRouting();

        //CORS
        app.UseCors("Default");

        //多语言
        app.UseLocalization();

        //认证
        app.UseAuthentication();

        //授权
        app.UseAuthorization();

        //配置端点
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        //启用Swagger
        app.UseSwagger(modules, options, app.Environment);

        //使用模块化
        app.UseModules(modules);

        //启用Banner图
        app.UseBanner(app.Lifetime, options);

        //启用应用关闭处理
        app.UseShutdownHandler();
    }

    /// <summary>
    /// 加载宿主配置项
    /// </summary>
    /// <returns></returns>
    private HostOptions LoadOptions()
    {
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false);

        var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environmentVariable.NotNull())
        {
            configBuilder.AddJsonFile($"appsettings.{environmentVariable}.json", false);
        }

        var config = configBuilder.Build();

        var hostOptions = new HostOptions();
        config.GetSection("Host").Bind(hostOptions);

        //设置默认端口
        if (hostOptions.Urls.IsNull())
            hostOptions.Urls = "http://*:6220";

        return hostOptions;
    }
}