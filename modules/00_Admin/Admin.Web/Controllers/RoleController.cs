using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mkh.Mod.Admin.Core.Application.Role;
using Mkh.Mod.Admin.Core.Application.Role.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace Mkh.Mod.Admin.Web.Controllers;

[SwaggerTag("角色管理")]
public class RoleController : Web.ModuleController
{
    private readonly IRoleService _service;

    public RoleController(IRoleService service)
    {
        _service = service;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <remarks>查询角色列表</remarks>
    [HttpGet]
    public Task<IResultModel> Query()
    {
        return _service.Query();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel> Add(RoleAddDto dto)
    {
        return _service.Add(dto);
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResultModel> Edit(int id)
    {
        return _service.Edit(id);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel> Update(RoleUpdateDto dto)
    {
        return _service.Update(dto);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpDelete]
    public Task<IResultModel> Delete([BindRequired] int id)
    {
        return _service.Delete(id);
    }
}