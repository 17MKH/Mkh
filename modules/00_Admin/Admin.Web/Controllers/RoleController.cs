using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Mod.Admin.Core.Application.Roles;
using Mkh.Mod.Admin.Core.Application.Roles.Dto;
using Mkh.Mod.Admin.Core.Application.Roles.Rto;
using Role = Mkh.Mod.Admin.Core.Domain.Roles.Role;

namespace Mkh.Mod.Admin.Web.Controllers;

[Tags("角色管理")]
public class RoleController : Web.ModuleController
{
    private readonly IRoleService _service;

    public RoleController(IRoleService service)
    {
        _service = service;
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel<RoleDetailsRto>> Create(RoleCreateDto dto)
    {
        return Success(_service.Create(dto));
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResultModel<RoleUpdateDto>> Edit(int id)
    {
        return Success(_service.Edit(id));
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel> Update(RoleUpdateDto dto)
    {
        return Success(_service.Update(dto));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpDelete]
    public Task<IResultModel> Delete([BindRequired] int id)
    {
        return Success(_service.Delete(id));
    }

    /// <summary>
    /// 查询角色绑定菜单信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResultModel<RoleBindMenusUpdateDto>> QueryBindMenus([BindRequired] int id)
    {
        return Success(_service.QueryBindMenus(id));
    }

    /// <summary>
    /// 更新绑定菜单信息
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResultModel> UpdateBindMenus(RoleBindMenusUpdateDto dto)
    {
        return Success(_service.UpdateBindMenus(dto));
    }

    /// <summary>
    /// 下拉列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowWhenAuthenticated]
    public Task<IResultModel<IEnumerable<OptionResultModel>>> Select()
    {
        return Success(_service.Select());
    }
}
