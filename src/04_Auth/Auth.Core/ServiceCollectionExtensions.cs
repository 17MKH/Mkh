﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Abstractions.Options;
using Mkh.Auth.Core;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Mkh身份认证核心功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置属性</param>
        /// <returns></returns>
        public static MkhAuthBuilder AddMkhAuth(this IServiceCollection services, IConfiguration configuration)
        {
            //添加身份认证配置项
            services.Configure<AuthOptions>(configuration.GetSection("Mkh:Auth"));

            //添加http上下文访问器，用于获取认证信息
            services.AddHttpContextAccessor();

            //尝试添加账户信息
            services.TryAddSingleton<IAccount, Account>();

            //添加权限解析器
            services.AddSingleton<IPermissionResolver, PermissionResolver>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MKH", policy => policy.Requirements.Add(new PermissionRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            var builder = new MkhAuthBuilder(services, configuration);

            return builder;
        }
    }
}
