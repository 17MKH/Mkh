using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.Menu;
using Mkh.Mod.Admin.Core.Application.Menu.Dto;
using Mkh.Mod.Admin.Core.Domain.Menu;

namespace Mkh.Mod.Admin.Web.Controllers;

[Tags("菜单管理")]
public class MenuController : Web.ModuleController
{
    private readonly IMenuService _service;

    public MenuController(IMenuService service)
    {
        _service = service;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <remarks>查询角色列表</remarks>
    [HttpGet]
    public Task<PagingQueryResultModel<MenuEntity>> Query([FromQuery] MenuQueryDto dto)
    {
        return _service.Query(dto);
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel<int>> Add(MenuAddDto dto)
    {
        return Success(_service.Add(dto));
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResultModel<MenuUpdateDto>> Edit(int id)
    {
        return Success(_service.Edit(id));
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel> Update(MenuUpdateDto dto)
    {
        return Success(_service.Update(dto));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public Task<IResultModel> Delete([BindRequired] int id)
    {
        return Success(_service.Delete(id));
    }

    /// <summary>
    /// 获取菜单树
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<IResultModel<List<TreeResultModel<MenuEntity>>>> Tree([BindRequired] int groupId)
    {
        return Success(_service.GetTree(groupId));
    }

    /// <summary>
    /// 更改排序
    /// </summary>
    /// <param name="menus"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResultModel> UpdateSort(IList<MenuEntity> menus)
    {
        return Success(_service.UpdateSort(menus));
    }
}