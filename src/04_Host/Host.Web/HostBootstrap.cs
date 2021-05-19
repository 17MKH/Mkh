using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mkh.Host.Web
{
    /// <summary>
    /// 宿主启动器
    /// </summary>
    public class HostBootstrap
    {
        private readonly string[] _args;

        public HostBootstrap(string[] args)
        {
            _args = args;
        }

        /// <summary>
        /// 创建IHost
        /// </summary>
        /// <returns></returns>
        public void Run()
        {
            var options = LoadOptions();

            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(_args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   //将宿主配置项注入容器
                   webBuilder.ConfigureServices(services =>
                   {
                       services.AddSingleton(options);
                   });

                   webBuilder.UseStartup<Startup>();
                   webBuilder.UseUrls(options.Urls);
               })
               .Build()
               .Run();
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
}
