using Microsoft.Extensions.DependencyInjection;
using Mkh.Config.Abstractions;
using Mkh.Module.Abstractions;

namespace Mkh.Config.Core
{
    public static class ModuleConfigureContextExtensions
    {
        /// <summary>
        /// 获取配置实例
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TConfig GetConfig<TConfig>(this ModuleConfigureContext context) where TConfig : IConfig, new()
        {
            return context.Services.BuildServiceProvider().GetService<IConfigProvider>()!.Get<TConfig>();
        }
    }
}
