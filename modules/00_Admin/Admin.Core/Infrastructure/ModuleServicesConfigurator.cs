using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mkh.Module.Abstractions;

namespace Mkh.Mod.Admin.Core.Infrastructure
{
    public class ModuleServicesConfigurator : IModuleServicesConfigurator
    {
        public void Configure(IServiceCollection services, IHostEnvironment environment)
        {
            services.AddSingleton<IPasswordHandler, DefaultPasswordHandler>();
            services.AddSingleton<IVerifyCodeProvider, DefaultVerifyCodeProvider>();
            services.AddSingleton<ICredentialClaimExtender, DefaultCredentialClaimExtender>();
        }
    }
}
