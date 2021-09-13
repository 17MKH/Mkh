using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Jwt;
using Mkh.Mod.Admin.Core.Infrastructure.Defaults;
using Mkh.Module.Abstractions;
using Mkh.Utils.Config;

namespace Mkh.Mod.Admin.Core.Infrastructure
{
    internal class ModuleServicesConfigurator : IModuleServicesConfigurator
    {
        public void Configure(ModuleConfigureContext context)
        {
            var services = context.Services;
            services.AddSingleton<IPasswordHandler, DefaultPasswordHandler>();
            services.AddSingleton<IVerifyCodeProvider, DefaultVerifyCodeProvider>();
            services.AddScoped<ICredentialClaimExtender, DefaultCredentialClaimExtender>();
            services.AddScoped<IAccountProfileResolver, DefaultAccountProfileResolver>();
            services.AddScoped<IJwtTokenStorageProvider, DefaultJwtTokenStorageProvider>();
            services.AddScoped<IPermissionValidateHandler, DefaultPermissionValidateHandler>();
            services.AddScoped<IAccountPermissionResolver, DefaultAccountPermissionResolver>();

            AddConfigCore(context);
        }

        /// <summary>
        /// 添加配置功能
        /// </summary>
        /// <returns></returns>
        private void AddConfigCore(ModuleConfigureContext context)
        {
            var configProvider = new DefaultConfigProvider();
            foreach (var module in context.Modules)
            {
                var configType = module.LayerAssemblies.Core.GetTypes().FirstOrDefault(m => typeof(IConfig).IsImplementType(m));
                if (configType != null)
                {
                    var instance = Activator.CreateInstance(configType);
                    context.Configuration.GetSection($"Mkh:Modules:{module.Code}:Config").Bind(instance);

                    configProvider.Configs.Add(configType.TypeHandle, (IConfig)instance);
                }
            }

            context.Services.AddSingleton<IConfigProvider>(configProvider);
        }
    }
}
