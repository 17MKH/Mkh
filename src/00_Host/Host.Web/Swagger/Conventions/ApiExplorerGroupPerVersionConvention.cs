using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Mkh.Host.Web.Swagger.Conventions;

/// <summary>
/// API分组约定
/// </summary>
public class ApiExplorerGroupConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerType.Namespace.IsNull())
            return;

        /*
         按照命名规范，假如前缀定义 xx.xx, 如：xx.xx.Mod.{模块名}.Web 时，按array[2]索引取分组的方式，
         将导致分组冲突（No operations defined in spec）
         */
        var fname = controller.ControllerType.FullName;
        var reg = Regex.Match(fname, "[.]+Mod[.]");
        fname = reg.Success ? fname.Substring(reg.Index + reg.Length) : fname;
        string[] array = fname.Split('.');
        controller.ApiExplorer.GroupName = array.Length > 0 ? array[0].ToLower() : fname;
    }
}
