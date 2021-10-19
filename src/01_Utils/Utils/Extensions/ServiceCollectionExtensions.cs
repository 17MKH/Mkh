using System.Linq;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Mkh;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 从服务集合中获取服务实例，需确保服务一定存在
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static T GetService<T>(this IServiceCollection services)
    {
        return (T)services.LastOrDefault(m => m.ServiceType == typeof(T))!.ImplementationInstance;
    }
}