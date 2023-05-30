using Microsoft.AspNetCore.Mvc;

namespace Mkh.Module.Web;

/// <summary>
/// 模块MVC配置项配置器接口
/// <para>当模块中有需要配置MVC功能时，可通过实现该接口来配置</para>
/// </summary>
public interface IModuleMvcOptionsConfigurator
{
    /// <summary>
    /// 配置MVC
    /// </summary>
    /// <param name="mvcOptions"></param>
    void Configure(MvcOptions mvcOptions);
}