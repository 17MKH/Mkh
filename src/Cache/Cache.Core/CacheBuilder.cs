using Microsoft.Extensions.DependencyInjection;
using Mkh.Cache.Abstractions;

namespace Mkh.Cache.Core
{
    internal class CacheBuilder : ICacheBuilder
    {
        public CacheBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public void Build()
        {
            //暂时没什么可以构造的
        }
    }
}
