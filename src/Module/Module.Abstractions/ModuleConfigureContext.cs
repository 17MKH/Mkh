using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mkh.Module.Abstractions;

/// <summary>
/// 用于模块中添加特有服务的上下文
/// </summary>
public class ModuleConfigureContext
{
    public ModuleConfigureContext(IServiceCollection services, IHostEnvironment environment, IConfiguration configuration, IModuleCollection modules)
    {
        Services = services;
        Environment = environment;
        Configuration = configuration;
        Modules = modules;
    }

    /// <summary>
    /// 服务集合
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// 环境变量
    /// </summary>
    public IHostEnvironment Environment { get; }

    /// <summary>
    /// 配置对象
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// 模块集合
    /// </summary>
    public IModuleCollection Modules { get; }
}