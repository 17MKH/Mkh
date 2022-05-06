using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Utils.Annotations;
using Mkh.Utils.Helpers;

namespace Mkh.Utils;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 从指定程序集中注入服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IServiceCollection AddServicesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            #region ==单例注入==

            var singletonAttr = (SingletonInjectAttribute)Attribute.GetCustomAttribute(type, typeof(SingletonInjectAttribute));
            if (singletonAttr != null)
            {
                //注入自身类型
                if (singletonAttr.Itself)
                {
                    services.AddSingleton(type);
                    continue;
                }

                var interfaces = type.GetInterfaces().Where(m => m != typeof(IDisposable)).ToList();
                if (interfaces.Any())
                {
                    foreach (var i in interfaces)
                    {
                        services.AddSingleton(i, type);
                    }
                }
                else
                {
                    services.AddSingleton(type);
                }

                continue;
            }

            #endregion

            #region ==瞬时注入==

            var transientAttr = (TransientInjectAttribute)Attribute.GetCustomAttribute(type, typeof(TransientInjectAttribute));
            if (transientAttr != null)
            {
                //注入自身类型
                if (transientAttr.Itself)
                {
                    services.AddSingleton(type);
                    continue;
                }

                var interfaces = type.GetInterfaces().Where(m => m != typeof(IDisposable)).ToList();
                if (interfaces.Any())
                {
                    foreach (var i in interfaces)
                    {
                        services.AddTransient(i, type);
                    }
                }
                else
                {
                    services.AddTransient(type);
                }
                continue;
            }

            #endregion

            #region ==Scoped注入==
            var scopedAttr = (ScopedInjectAttribute)Attribute.GetCustomAttribute(type, typeof(ScopedInjectAttribute));
            if (scopedAttr != null)
            {
                //注入自身类型
                if (scopedAttr.Itself)
                {
                    services.AddSingleton(type);
                    continue;
                }

                var interfaces = type.GetInterfaces().Where(m => m != typeof(IDisposable)).ToList();
                if (interfaces.Any())
                {
                    foreach (var i in interfaces)
                    {
                        services.AddScoped(i, type);
                    }
                }
                else
                {
                    services.AddScoped(type);
                }
            }

            #endregion
        }

        return services;
    }

    /// <summary>
    /// 扫描并注入所有使用特性注入的服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServicesFromAttribute(this IServiceCollection services)
    {
        //就是要硬编码Mkh.，任性~
        var assemblies = new AssemblyHelper().Load(m => m.Name.StartsWith("Mkh."));
        foreach (var assembly in assemblies)
        {
            try
            {
                services.AddServicesFromAssembly(assembly);
            }
            catch
            {
                //此处防止第三方库抛出一场导致系统无法启动，所以需要捕获异常来处理一下
            }
        }
        return services;
    }

    /// <summary>
    /// 添加Utils中的服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static IServiceCollection AddUtils(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddServicesFromAttribute();

        return services;
    }
}