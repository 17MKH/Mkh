using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Mkh.Module.Web;

/// <summary>
/// 模块中间件配置器接口
/// <para>当模块中包含独有的中间件时，可以通过实现该接口来添加</para>
/// </summary>
public interface IModuleMiddlewareConfigurator
{
    /// <summary>
    /// 配置中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    void PreConfigure(IApplicationBuilder app, IHostEnvironment env);

    /// <summary>
    /// 配置中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    void Configure(IApplicationBuilder app, IHostEnvironment env);

    /// <summary>
    /// 配置中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    void PostConfigure(IApplicationBuilder app, IHostEnvironment env);
}