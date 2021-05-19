using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mkh.Host.Web.Middlewares;
using Mkh.Host.Web.Swagger;
using Mkh.Host.Web.Swagger.Conventions;
using Mkh.Module.Abstractions;
using Mkh.Module.Core;
using Mkh.Module.Web;
using Mkh.Utils;

namespace Mkh.Host.Web
{
    public class Startup
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
            services.AddSwagger(modules, hostOptions);

            //添加MVC配置
            services.AddMvc(c =>
                {
                    if (hostOptions!.Swagger || _env.IsDevelopment())
                    {
                        //API分组约定
                        c.Conventions.Add(new ApiExplorerGroupConvention());
                    }
                })
                .AddApplicationPart(modules[0].LayerAssemblies.Web)
                //添加模块
                .AddModules(modules);

            //添加CORS
            services.AddCors(options =>
            {
                /*浏览器的同源策略，就是出于安全考虑，浏览器会限制从脚本发起的跨域HTTP请求（比如异步请求GET, POST, PUT, DELETE, OPTIONS等等，
                        所以浏览器会向所请求的服务器发起两次请求，第一次是浏览器使用OPTIONS方法发起一个预检请求，第二次才是真正的异步请求，
                        第一次的预检请求获知服务器是否允许该跨域请求：如果允许，才发起第二次真实的请求；如果不允许，则拦截第二次请求。
                        Access-Control-Max-Age用来指定本次预检请求的有效期，单位为秒，，在此期间不用发出另一条预检请求。*/
                var preflightMaxAge = hostOptions.PreflightMaxAge < 0 ? new TimeSpan(0, 30, 0) : new TimeSpan(0, 0, hostOptions.PreflightMaxAge);

                options.AddPolicy("Default",
                    builder => builder.AllowAnyOrigin()
                        .SetPreflightMaxAge(preflightMaxAge)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Disposition"));//下载文件时，文件名称会保存在headers的Content-Disposition属性里面
            });

            //解决Multipart body length limit 134217728 exceeded
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            //添加HttpClient相关
            services.AddHttpClient();

            //添加模块的自定义服务
            services.AddModuleServices(modules);
        }

        public virtual void Configure(IApplicationBuilder app, IHostApplicationLifetime appLifetime)
        {
            var hostOptions = app.ApplicationServices.GetService<HostOptions>();
            var modules = app.ApplicationServices.GetRequiredService<IModuleCollection>();

            var partManager = app.ApplicationServices.GetService<ApplicationPartManager>();

            //使用全局异常处理中间件
            app.UseMiddleware<ExceptionHandleMiddleware>();

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
            app.UseSwagger(modules, hostOptions);

            //使用模块化
            app.UseModules(modules);

            appLifetime.ApplicationStarted.Register(() =>
            {
                //显示启动Banner
                var options = app.ApplicationServices.GetService<HostOptions>();
                ConsoleBanner(options);
            });
        }

        /// <summary>
        /// 启动后打印Banner图案
        /// </summary>
        /// <param name="options"></param>
        private void ConsoleBanner(HostOptions options)
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
            Console.Write(@" *                                      ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(@"接口地址：" + options.Urls);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"                                                *");
            Console.WriteLine(@" *                                                                                                             *");
            Console.WriteLine(@" ***************************************************************************************************************");
            Console.WriteLine();
        }
    }
}
