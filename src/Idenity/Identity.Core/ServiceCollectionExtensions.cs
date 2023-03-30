using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh.Identity.Abstractions;

namespace Mkh.Identity.Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加身份服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            //添加http上下文访问器，用于获取认证信息
            services.AddHttpContextAccessor();

            //尝试添加账户信息
            services.TryAddSingleton<ICurrentAccount, CurrentAccount>();

            return services;
        }
    }
}
