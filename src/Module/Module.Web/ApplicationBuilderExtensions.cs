using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Mkh.Module.Abstractions;

namespace Mkh.Module.Web;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// 使用模块的中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="modules">模块集合</param>
    /// <param name="env"></param>
    /// <returns></returns>
    public static List<IModuleMiddlewareConfigurator> UseModules(this IApplicationBuilder app, IModuleCollection modules, IHostEnvironment env)
    {
        var moduleMiddlewareConfigurators = new List<IModuleMiddlewareConfigurator>();

        foreach (var module in modules)
        {
            if (module?.LayerAssemblies == null)
                continue;

            var assembly = module.LayerAssemblies.Web ?? module.LayerAssemblies.Api;
            if (assembly == null)
                continue;

            var middlewareConfigurator = assembly.GetTypes().FirstOrDefault(m => typeof(IModuleMiddlewareConfigurator).IsAssignableFrom(m));
            if (middlewareConfigurator != null)
            {
                var instance = (IModuleMiddlewareConfigurator)Activator.CreateInstance(middlewareConfigurator);
                if (instance != null)
                    moduleMiddlewareConfigurators.Add(instance);
            }
        }

        return moduleMiddlewareConfigurators;
    }
}