using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Module.Abstractions;

namespace Mkh.Module.Web;

public static class MvcBuilderExtensions
{
    /// <summary>
    /// 添加模块
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="modules"></param>
    /// <returns></returns>
    public static IModuleCollection AddModules(this IMvcBuilder builder, IModuleCollection modules)
    {
        foreach (var module in modules)
        {
            var assembly = module.LayerAssemblies.Web ?? module.LayerAssemblies.Api;
            if (assembly == null)
                continue;

            if (module?.LayerAssemblies == null)
                continue;

            builder.AddApplicationPart(module.LayerAssemblies.Web ?? module.LayerAssemblies.Api);

            builder.AddMvcOptions(options =>
            {
                var mvcOptionsConfigurator = assembly.GetTypes().FirstOrDefault(m => typeof(IModuleMvcOptionsConfigurator).IsAssignableFrom(m));
                if (mvcOptionsConfigurator != null)
                {
                    ((IModuleMvcOptionsConfigurator)Activator.CreateInstance(mvcOptionsConfigurator))?.Configure(options);
                }
            });
        }

        return modules;
    }
}