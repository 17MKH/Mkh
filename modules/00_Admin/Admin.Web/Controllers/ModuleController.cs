using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mkh.Auth.Abstractions;
using Swashbuckle.AspNetCore.Annotations;

namespace Mkh.Mod.Admin.Web.Controllers;

[SwaggerTag("模块管理")]
public class ModuleController : Web.ModuleController
{
    private readonly IPermissionResolver _permissionResolver;

    public ModuleController(IPermissionResolver permissionResolver)
    {
        _permissionResolver = permissionResolver;
    }

    /// <summary>
    /// 获取指定模块的权限列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IResultModel<IList<PermissionDescriptor>> Permissions([BindRequired] string moduleCode)
    {
        return ResultModel.Success(_permissionResolver.GetPermissions(moduleCode));
    }
}