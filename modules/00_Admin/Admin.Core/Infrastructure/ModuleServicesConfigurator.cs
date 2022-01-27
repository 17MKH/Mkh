using Microsoft.Extensions.DependencyInjection;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Jwt;
using Mkh.Mod.Admin.Core.Infrastructure.Defaults;
using Mkh.Module.Abstractions;

namespace Mkh.Mod.Admin.Core.Infrastructure;

public class ModuleServicesConfigurator : IModuleServicesConfigurator
{
    public void Configure(ModuleConfigureContext context)
    {
        var services = context.Services;
        services.AddSingleton<IPasswordHandler, DefaultPasswordHandler>();
        services.AddSingleton<IVerifyCodeProvider, DefaultVerifyCodeProvider>();
        services.AddScoped<ICredentialClaimExtender, DefaultCredentialClaimExtender>();
        services.AddScoped<IAccountProfileResolver, DefaultAccountProfileResolver>();
        services.AddScoped<IJwtTokenStorage, AdminJwtTokenStorage>();
        services.AddScoped<IPermissionValidateHandler, AdminPermissionValidateHandler>();
        services.AddScoped<IAccountPermissionResolver, DefaultAccountPermissionResolver>();
    }
}