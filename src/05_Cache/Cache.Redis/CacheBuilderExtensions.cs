using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh.Cache.Abstractions;
using Mkh.Cache.Redis;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CacheBuilderExtensions
{
    /// <summary>
    /// 添加Redis缓存
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static CacheBuilder UseRedis(this CacheBuilder builder, IConfiguration cfg)
    {
        var services = builder.Services;

        services.Configure<RedisOptions>(cfg.GetSection("Mkh:Cache:Redis"));

        services.TryAddSingleton<IRedisSerializer, DefaultRedisSerializer>();

        services.AddSingleton<RedisHelper>();

        services.AddSingleton<ICacheHandler, RedisCacheHandler>();

        return builder;
    }
}