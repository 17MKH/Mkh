using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Mkh.Host.Web.Middleware;
using Mkh.Host.Web.Swagger;
using Mkh.Module.Abstractions;
using Mkh.Module.Core;
using Mkh.Module.Web;
using Mkh.Utils;
using Mkh.Utils.App;
using HostOptions = Mkh.Host.Web.Options.HostOptions;

namespace Mkh.Host.Web;

internal class Startup
{
    private readonly IHostEnvironment _env;
    private readonly IConfiguration _cfg;

    public Startup(IHostEnvironment env, IConfiguration cfg)
    {
        _env = env;
        _cfg = cfg;
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        var hostOptions = services.GetService<HostOptions>();

        //注入服务
        services.AddServicesFromAttribute();

        //添加模块
        var modules = services.AddModulesCore(_env, _cfg);

        //添加Swagger
        services.AddSwagger(modules, hostOptions, _env);

        //添加缓存
        services.AddCache(_cfg);

        //添加对象映射
        services.AddMappers(modules);

        //添加MVC配置
        services.AddMvc(modules, hostOptions, _env);

        //添加CORS
        services.AddCors(hostOptions);

        //解决Multipart body length limit 134217728 exceeded
        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = int.MaxValue;
        });

        //添加HttpClient相关
        services.AddHttpClient();

        //添加模块的自定义特有的服务
        services.AddModuleServices(modules, _env, _cfg);

        //添加身份认证和授权
        services.AddMkhAuth(_cfg).UseJwt();

        //添加数据库
        services.AddData(modules);
    }

    public virtual void Configure(IApplicationBuilder app, IHostApplicationLifetime appLifetime)
    {
        var hostOptions = app.ApplicationServices.GetService<HostOptions>();
        var modules = app.ApplicationServices.GetRequiredService<IModuleCollection>();

        //使用全局异常处理中间件
        app.UseMiddleware<ExceptionHandleMiddleware>();

        //基地址
        app.UsePathBase(hostOptions);

        //配置默认页
        app.UseDefaultPage();

        //反向代理
        if (hostOptions!.Proxy)
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
        app.UseSwagger(modules, hostOptions, _env);

        //使用模块化
        app.UseModules(modules);

        //启用Banner图
        app.UseBanner(appLifetime);

        //启用应用关闭处理
        app.UseShutdownHandler();
    }
}