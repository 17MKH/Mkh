using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Core;
using Mkh.Auth.Jwt;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class JwtExtensions
    {
        /// <summary>
        /// 使用Jwt身份认证方案
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static MkhAuthBuilder UseJwt(this MkhAuthBuilder builder, Action<JwtBearerOptions> configure = null)
        {
            var services = builder.Services;

            //添加Jwt配置项
            var jwtOptions = new JwtOptions();
            builder.Configuration.GetSection("Mkh:Auth:Jwt").Bind(jwtOptions);
            services.AddSingleton(jwtOptions);

            //添加凭证构造器
            services.AddScoped<ICredentialBuilder, JwtCredentialBuilder>();

            //添加身份认证服务
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //配置令牌验证参数
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key)),
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience
                    };

                    //自定义配置
                    configure?.Invoke(options);
                });


            return builder;
        }
    }
}
