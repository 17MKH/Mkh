using System.Collections.Generic;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.Menu.Dto;
using Mkh.Mod.Admin.Core.Domain.Menu;

namespace Mkh.Mod.Admin.Core.Application.Menu;

/// <summary>
/// 菜单服务
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <returns></returns>
    Task<PagingQueryResultModel<MenuEntity>> Query(MenuQueryDto dto);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<IResultModel> Add(MenuAddDto dto);

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResultModel> Edit(int id);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<IResultModel> Update(MenuUpdateDto dto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResultModel> Delete(int id);

    /// <summary>
    /// 查询菜单树
    /// </summary>
    /// <param name="groupId">菜单分组编号</param>
    /// <returns></returns>
    Task<IResultModel> GetTree(int groupId);

    /// <summary>
    /// 修改菜单排序
    /// </summary>
    /// <param name="menus"></param>
    /// <returns></returns>
    Task<IResultModel> UpdateSort(IList<MenuEntity> menus);
}