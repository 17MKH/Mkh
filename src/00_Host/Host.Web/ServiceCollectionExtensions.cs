using System;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mkh.Data.Core;
using Mkh.Host.Web.Swagger.Conventions;
using Mkh.Module.Abstractions;
using Mkh.Module.Web;
using Mkh.Utils.Mapper;

namespace Mkh.Host.Web
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加对象映射
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules">模块集合</param>
        /// <returns></returns>
        public static IServiceCollection AddMappers(this IServiceCollection services, IModuleCollection modules)
        {
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var module in modules)
                {
                    var types = module.LayerAssemblies.Core.GetTypes().Where(t => typeof(IMapperConfig).IsAssignableFrom(t));

                    foreach (var type in types)
                    {
                        ((IMapperConfig)Activator.CreateInstance(type))!.Bind(cfg);
                    }
                }
            });

            services.AddSingleton(config.CreateMapper());

            return services;
        }

        /// <summary>
        /// 添加MVC功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IServiceCollection AddMvc(this IServiceCollection services, IModuleCollection modules, Options.HostOptions hostOptions, IHostEnvironment env)
        {
            services.AddMvc(c =>
                {
                    if (hostOptions!.Swagger || env.IsDevelopment())
                    {
                        //API分组约定
                        c.Conventions.Add(new ApiExplorerGroupConvention());
                    }
                })
                //添加模块
                .AddModules(modules);

            return services;
        }

        /// <summary>
        /// 添加CORS
        /// </summary>
        /// <param name="services"></param>
        /// <param name="hostOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddCors(this IServiceCollection services, Options.HostOptions hostOptions)
        {
            services.AddCors(options =>
            {
                /*浏览器的同源策略，就是出于安全考虑，浏览器会限制从脚本发起的跨域HTTP请求（比如异步请求GET, POST, PUT, DELETE, OPTIONS等等，
                        所以浏览器会向所请求的服务器发起两次请求，第一次是浏览器使用OPTIONS方法发起一个预检请求，第二次才是真正的异步请求，
                        第一次的预检请求获知服务器是否允许该跨域请求：如果允许，才发起第二次真实的请求；如果不允许，则拦截第二次请求。
                        Access-Control-Max-Age用来指定本次预检请求的有效期，单位为秒，，在此期间不用发出另一条预检请求。*/
                var preflightMaxAge = hostOptions.PreflightMaxAge > 0 ? new TimeSpan(0, 0, hostOptions.PreflightMaxAge) : new TimeSpan(0, 30, 0);

                options.AddPolicy("Default",
                    builder => builder.SetIsOriginAllowed(_ => true)
                        .SetPreflightMaxAge(preflightMaxAge)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition"));//下载文件时，文件名称会保存在headers的Content-Disposition属性里面
            });

            return services;
        }

        /// <summary>
        /// 添加数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddData(this IServiceCollection services, IModuleCollection modules)
        {
            foreach (var module in modules)
            {
                var dbOptions = module.Options!.Db;
                var dbContextType = module.LayerAssemblies.Core.GetTypes().FirstOrDefault(m => typeof(DbContext).IsAssignableFrom(m));

                var dbBuilder = services.AddMkhDb(dbContextType, opt =>
                {
                    opt.Provider = dbOptions.Provider;
                    opt.ConnectionString = dbOptions.ConnectionString;
                    opt.Log = dbOptions.Log;
                    opt.TableNamePrefix = dbOptions.TableNamePrefix;
                    opt.TableNameSeparator = dbOptions.TableNameSeparator;
                    opt.Version = dbOptions.Version;
                });

                //加载仓储
                dbBuilder.AddRepositoriesFromAssembly(module.LayerAssemblies.Core);

                //启用代码优先
                if (dbOptions.CodeFirst)
                {
                    dbBuilder.AddCodeFirst(opt =>
                    {
                        opt.CreateDatabase = dbOptions.CreateDatabase;
                        opt.UpdateColumn = dbOptions.UpdateColumn;
                    });
                }

                //特性事务
                foreach (var dic in module.ApplicationServices)
                {
                    dbBuilder.AddTransactionAttribute(dic.Key, dic.Value);
                }

                dbBuilder.Build();
            }

            return services;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration cfg)
        {
            var builder = services.AddCache();

            var provider = cfg["Mkh:Cache:Provider"].ToInt();
            if (provider == 1)
            {
                builder.UseRedis(cfg);
            }

            return services;
        }
    }
}
