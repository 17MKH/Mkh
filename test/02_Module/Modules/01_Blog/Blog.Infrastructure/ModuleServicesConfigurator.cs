using Microsoft.Extensions.DependencyInjection;
using Mkh.Module.Abstractions;
using Mkh.Module.Blog.Infrastructure.ModuleServiceTest;

namespace Mkh.Module.Blog.Infrastructure
{
    public class ModuleServicesConfigurator : IModuleServicesConfigurator
    {
        public void Configure(ModuleConfigureContext context)
        {

            /*
             * 通过实现IModuleServicesConfigurator接口，可以在模块中添加模块独有的一些服务
             *
             * 当然，大部分服务其实可以通过附加SingletonAttribute特性自动注入，不需要实现该接口，这里只是演示该功能
             *
             */

            var services = context.Services;

            services.AddSingleton<IModuleServiceTest, ModuleServiceTest.ModuleServiceTest>();
        }
    }
}
