using Microsoft.Extensions.DependencyInjection;

namespace Mkh.Cache.Abstractions
{
    /// <summary>
    /// 缓存构造器
    /// </summary>
    public interface ICacheBuilder
    {
        /// <summary>
        /// 服务
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// 构造
        /// </summary>
        void Build();
    }
}
