using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Mkh.Module.Abstractions.Options;
using Mkh.Utils.App;
using HostOptions = Mkh.Host.Web.Options.HostOptions;

namespace Mkh.Host.Web;

internal static class ApplicationBuilderExtensions
{
    /// <summary>
    /// 配置路基跟地址
    /// </summary>
    /// <param name="app"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IApplicationBuilder UsePathBase(this IApplicationBuilder app, Options.HostOptions options)
    {
        var pathBase = options!.Base;
        if (pathBase.NotNull())
        {
            //1、配置请求基础地址：
            app.Use((context, next) =>
            {
                context.Request.PathBase = pathBase;
                return next();
            });

            // 2、配置静态文件基地址：
            app.UsePathBase(pathBase);
        }

        return app;
    }

    /// <summary>
    /// 设置默认页为index.html
    /// </summary>
    /// <param name="app"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDefaultPage(this IApplicationBuilder app, Options.HostOptions options)
    {
        //设置默认文档
        var defaultFilesOptions = new DefaultFilesOptions();
        defaultFilesOptions.DefaultFileNames.Clear();
        defaultFilesOptions.DefaultFileNames.Add("index.html");
        app.UseDefaultFiles(defaultFilesOptions);

        if (options.DefaultDir.NotNull())
        {
            var rewriteOptions = new RewriteOptions().AddRedirect("^$", options.DefaultDir);
            app.UseRewriter(rewriteOptions);
        }

        return app;
    }

    /// <summary>
    /// 开放wwwroot下指定目录
    /// </summary>
    /// <param name="app"></param>
    /// <param name="dirName"></param>
    /// <param name="env"></param>
    /// <returns></returns>
    public static IApplicationBuilder OpenDir(this IApplicationBuilder app, string dirName, IWebHostEnvironment env)
    {
        if (env.WebRootPath.IsNull())
            return app;

        var path = Path.Combine(env.WebRootPath, dirName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var options = new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(path),
            RequestPath = new PathString("/" + dirName)
        };

        app.UseStaticFiles(options);

        return app;
    }

    /// <summary>
    /// 启用应用停止处理
    /// </summary>
    /// <returns></returns>
    public static IApplicationBuilder UseShutdownHandler(this IApplicationBuilder app)
    {
        var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        applicationLifetime.ApplicationStopping.Register(() =>
        {
            var handlers = app.ApplicationServices.GetServices<IAppShutdownHandler>().ToList();
            foreach (var handler in handlers)
            {
                handler.Handle();
            }
        });

        return app;
    }

    /// <summary>
    /// 启用Banner图
    /// </summary>
    /// <param name="app"></param>
    /// <param name="appLifetime"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseBanner(this IApplicationBuilder app, IHostApplicationLifetime appLifetime, HostOptions options)
    {
        appLifetime.ApplicationStarted.Register(() =>
        {
            //显示启动Banner
            var customFile = Path.Combine(AppContext.BaseDirectory, "banner.txt");
            if (File.Exists(customFile))
            {
                try
                {
                    var lines = File.ReadAllLines(customFile);
                    foreach (var line in lines)
                    {
                        Console.WriteLine(line);
                    }
                }
                catch
                {
                    Console.WriteLine("banner.txt文件无效");
                }
            }
            else
            {
                ConsoleBanner(options);
            }
        });

        return app;
    }

    /// <summary>
    /// 启用多语言中间件
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseLocalization(this IApplicationBuilder app)
    {
        var commonOptions = app.ApplicationServices.GetService<CommonOptions>();

        //多语言
        var supportedCultures = new[] { "zh", "en" };
        var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(commonOptions!.Lang ?? supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        //移除默认的语言解析器，只保留从Accept-Language请求头解析
        var acceptLanguageHeaderRequestCultureProvider = localizationOptions.RequestCultureProviders[2];
        localizationOptions.RequestCultureProviders.RemoveAt(0);
        localizationOptions.RequestCultureProviders = new List<IRequestCultureProvider> { acceptLanguageHeaderRequestCultureProvider };

        app.UseRequestLocalization(localizationOptions);

        return app;
    }

    /// <summary>
    /// 启动后打印Banner图案
    /// </summary>
    private static void ConsoleBanner(HostOptions options)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        Console.WriteLine(@" ***************************************************************************************************************");
        Console.WriteLine(@" *                                                                                                             *");
        Console.WriteLine(@" *                               $$\   $$$$$$$$\ $$\      $$\ $$\   $$\ $$\   $$\                              *");
        Console.WriteLine(@" *                             $$$$ |  \____$$  |$$$\    $$$ |$$ | $$  |$$ |  $$ |                             *");
        Console.WriteLine(@" *                             \_$$ |      $$  / $$$$\  $$$$ |$$ |$$  / $$ |  $$ |                             *");
        Console.WriteLine(@" *                               $$ |     $$  /  $$\$$\$$ $$ |$$$$$  /  $$$$$$$$ |                             *");
        Console.WriteLine(@" *                               $$ |    $$  /   $$ \$$$  $$ |$$  $$<   $$  __$$ |                             *");
        Console.WriteLine(@" *                               $$ |   $$  /    $$ |\$  /$$ |$$ |\$$\  $$ |  $$ |                             *");
        Console.WriteLine(@" *                             $$$$$$\ $$  /     $$ | \_/ $$ |$$ | \$$\ $$ |  $$ |                             *");
        Console.WriteLine(@" *                             \______|\__/      \__|     \__|\__|  \__|\__|  \__|                             *");
        Console.WriteLine(@" *                                                                                                             *");
        Console.WriteLine(@" *                                                                                                             *");
        Console.Write(@" *                                      ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(@"启动成功，欢迎使用 17MKH ~");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(@"                                             *");
        Console.WriteLine(@" *                                                                                                             *");
        Console.Write(@" *                                          ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(@"URL：" + options.Urls);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(@"                                                 *");
        Console.WriteLine(@" *                                                                                                             *");
        Console.WriteLine(@" *                                                                                                             *");
        Console.WriteLine(@" ***************************************************************************************************************");
        Console.WriteLine();
    }
}