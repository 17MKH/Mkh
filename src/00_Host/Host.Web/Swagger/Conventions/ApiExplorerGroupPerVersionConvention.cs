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

        string[] array = controller.ControllerType.FullName.Split('.');
        controller.ApiExplorer.GroupName = array[2].ToLower();
    }
}