using System;
using AutoMapper;
using Mkh.Mapper;
using Mkh.Module.Abstractions;
using Mkh.Utils.Annotations;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加对象映射
    /// </summary>
    /// <param name="services"></param>
    /// <param name="modules">模块集合</param>
    /// <returns></returns>
    public static IServiceCollection AddMappers(this IServiceCollection services, IModuleCollection modules)
    {
        var config = new MapperConfiguration(cfg =>
        {
            foreach (var module in modules)
            {
                var types = module.LayerAssemblies.Core.GetTypes();

                foreach (var type in types)
                {
                    var map = (ObjectMapAttribute)Attribute.GetCustomAttribute(type, typeof(ObjectMapAttribute));
                    if (map != null)
                    {
                        cfg.CreateMap(type, map.TargetType);

                        if (map.TwoWay)
                        {
                            cfg.CreateMap(map.TargetType, type);
                        }
                    }
                }
            }
        });

        services.AddSingleton(config.CreateMapper());
        services.AddSingleton<Mkh.Utils.Map.IMapper, DefaultMapper>();

        return services;
    }
}