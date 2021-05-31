using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mkh.Module.Abstractions
{
    /// <summary>
    /// 模块服务配置器接口
    /// <para>如果模块中有自己独有的服务需要注入，可以通过实现该接口来注入</para>
    /// </summary>
    public interface IModuleServicesConfigurator
    {
        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        void Configure(IServiceCollection services, IHostEnvironment environment);
    }
}
