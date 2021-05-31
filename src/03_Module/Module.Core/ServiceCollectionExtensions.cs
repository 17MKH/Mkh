using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mkh.Module.Abstractions;
using Mkh.Module.Abstractions.Options;
using Mkh.Utils.Extensions;

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
        /// <param name="codes"></param>
        /// <returns></returns>
        public static IModuleCollection AddModulesCore(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
        {
            var moduleOptionsList = configuration.Get<List<ModuleOptions>>("Mkh:Modules");

            var modules = new ModuleCollection(environment);
            modules.Load(moduleOptionsList);

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

                //加载模块初始化器
                module.ServicesConfigurator?.Configure(services, modules.HostEnvironment);

                services.AddApplicationServices(module);
            }

            return services;
        }

        /// <summary>
        /// 添加应用服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="module"></param>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, ModuleDescriptor module)
        {
            var assembly = module.LayerAssemblies.Core;
            //按照约定，应用服务必须采用Service结尾
            var implementationTypes = assembly.GetTypes().Where(m => m.Name.EndsWith("Service") && !m.IsInterface).ToList();

            foreach (var implType in implementationTypes)
            {
                //按照约定，服务的第一个接口类型就是所需的应用服务接口
                var serviceType = implType.GetInterfaces()[0];

                services.AddScoped(implType);

                module.ApplicationServices.Add(serviceType, implType);
            }

            return services;
        }
    }
}
