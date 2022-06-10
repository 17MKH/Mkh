namespace Mkh.Module.Abstractions;

/// <summary>
/// 模块服务配置器接口
/// <para>如果模块中有自己独有的服务需要注入，可以通过实现该接口来注入</para>
/// </summary>
public interface IModuleServicesConfigurator
{
    /// <summary>
    /// 前置服务注入
    /// </summary>
    /// <param name="context"></param>
    void PreConfigure(ModuleConfigureContext context);

    /// <summary>
    /// 服务注入
    /// </summary>
    /// <param name="context"></param>
    void Configure(ModuleConfigureContext context);

    /// <summary>
    /// 后置服务注入
    /// </summary>
    /// <param name="context"></param>
    void PostConfigure(ModuleConfigureContext context);
}