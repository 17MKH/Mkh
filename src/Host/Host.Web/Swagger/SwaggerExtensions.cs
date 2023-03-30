using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mkh.Host.Web.Swagger.Filters;
using Mkh.Module.Abstractions;
using HostOptions = Mkh.Host.Web.Options.HostOptions;

namespace Mkh.Host.Web.Swagger;

public static class SwaggerExtensions
{
    /// <summary>
    /// 添加Swagger
    /// </summary>
    /// <param name="services"></param>
    /// <param name="modules">模块集合</param>
    /// <param name="hostOptions"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services, IModuleCollection modules, HostOptions hostOptions, IHostEnvironment environment)
    {
        if (Check(modules, hostOptions, environment))
        {
            services.AddSwaggerGen(c =>
            {
                if (modules != null)
                {
                    foreach (var module in modules)
                    {
                        c.SwaggerDoc(module.Code.ToLower(), new OpenApiInfo
                        {
                            Title = $"{module.Name}({module.Code})",
                            Version = module.Version,
                            Description = module.Description
                        });


                        //加载xml文档
                        var xmlPath = module.LayerAssemblies.Web.Location.Replace(".dll", ".xml", StringComparison.OrdinalIgnoreCase);
                        if (File.Exists(xmlPath))
                        {
                            c.IncludeXmlComments(xmlPath);
                        }
                    }
                }

                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT认证请求头格式: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                };

                //添加设置Token的按钮
                c.AddSecurityDefinition("Bearer", securityScheme);

                //添加Jwt验证设置
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                //启用注解
                c.EnableAnnotations();

                //隐藏属性
                c.SchemaFilter<SwaggerIgnoreSchemaFilter>();
                c.OperationFilter<SwaggerIgnoreOperationFilter>();
            });
        }

        return services;
    }

    /// <summary>
    /// 启用Swagger
    /// </summary>
    public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IModuleCollection modules, HostOptions hostOptions, IHostEnvironment environment)
    {
        //手动开启或者开发模式下才会启用swagger功能
        if (Check(modules, hostOptions, environment))
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                if (modules == null) return;

                foreach (var module in modules)
                {
                    var code = module.Code.ToLower();
                    var url = $"{code}/swagger.json";

                    c.SwaggerEndpoint(url, $"{module.Name}({module.Code})");
                }
            });
        }

        return app;
    }

    /// <summary>
    /// 检测是否开启Swagger
    /// </summary>
    private static bool Check(IModuleCollection modules, HostOptions hostOptions, IHostEnvironment environment)
    {
        //手动开启或者开发模式以及本地模式下才会启用swagger功能
        return hostOptions.Swagger || !environment.IsProduction() || environment.EnvironmentName.Equals("Local");
    }
}