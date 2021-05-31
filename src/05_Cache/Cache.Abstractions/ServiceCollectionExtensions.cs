using Mkh.Cache.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Redis缓存
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static CacheBuilder AddCache(this IServiceCollection services)
        {
            services.AddSingleton<ICacheHandler, MemoryCacheHandler>();

            return new CacheBuilder { Services = services };
        }
    }
}
