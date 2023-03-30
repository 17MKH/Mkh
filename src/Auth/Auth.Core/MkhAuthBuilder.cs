using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mkh.Auth.Core;

public class MkhAuthBuilder
{
    public IServiceCollection Services { get; set; }

    /// <summary>
    /// 配置属性
    /// </summary>
    public IConfiguration Configuration { get; set; }

    public MkhAuthBuilder(IServiceCollection services, IConfiguration configuration)
    {
        Services = services;
        Configuration = configuration;
    }
}