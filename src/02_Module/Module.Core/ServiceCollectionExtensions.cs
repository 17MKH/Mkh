using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mkh.Module.Abstractions;

namespace Mkh.Module.Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加模块核心功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IModuleCollection AddModulesCore(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
        {
            var modules = new ModuleCollection(environment);
            modules.Load();

            services.AddSingleton<IModuleCollection>(modules);

            return modules;
        }

        /// <summary>
        /// 添加模块相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IServiceCollection AddModuleServices(this IServiceCollection services, IModuleCollection modules)
        {
            foreach (var module in modules)
            {
                if (module == null)
                    continue;

                services.AddApplicationServices(module);

                //加载模块初始化器
                module.ServicesConfigurator?.Configure(services, modules.HostEnvironment);
            }

            return services;
        }

        /// <summary>
        /// 添加应用服务
        /// </summary>
        private static void AddApplicationServices(this IServiceCollection services, ModuleDescriptor module)
        {
            if (module.LayerAssemblies == null || module.LayerAssemblies.Core == null)
                return;

            var types = module.LayerAssemblies.Core.GetTypes();
            var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.FullName.EndsWith("Service", StringComparison.OrdinalIgnoreCase));
            foreach (var serviceType in interfaces)
            {
                var implementationType = types.FirstOrDefault(m => m != serviceType && serviceType.IsAssignableFrom(m));
                if (implementationType != null)
                {
                    services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
                }
            }
        }
    }
}
