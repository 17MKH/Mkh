using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh.Cache.Abstractions;

namespace Mkh.Cache.Redis
{
    public static class CacheBuilderExtensions
    {
        /// <summary>
        /// 使用Redis缓存
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static ICacheBuilder UseRedis(this ICacheBuilder builder, IConfiguration cfg)
        {
            var services = builder.Services;

            services.Configure<RedisOptions>(cfg.GetSection("Mkh:Cache:Redis"));
            services.TryAddSingleton<IRedisSerializer, DefaultRedisSerializer>();
            services.AddSingleton<RedisHelper>();
            services.AddSingleton<ICacheProvider, RedisCacheProvider>();

            return builder;
        }
    }
}
