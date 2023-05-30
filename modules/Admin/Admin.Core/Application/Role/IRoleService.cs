using System.Collections.Generic;
using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Role.Dto;
using Mkh.Mod.Admin.Core.Domain.Role;

namespace Mkh.Mod.Admin.Core.Application.Role;

/// <summary>
/// 角色服务
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <returns></returns>
    Task<IResultModel<IList<RoleEntity>>> Query();

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<IResultModel> Add(RoleAddDto dto);

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
    Task<IResultModel> Update(RoleUpdateDto dto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResultModel> Delete(int id);

    /// <summary>
    /// 查询指定角色绑定的菜单信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResultModel> QueryBindMenus(int id);

    /// <summary>
    /// 更新角色绑定的菜单信息
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<IResultModel> UpdateBindMenus(RoleBindMenusUpdateDto dto);

    /// <summary>
    /// 下拉列表
    /// </summary>
    /// <returns></returns>
    Task<IResultModel> Select();
}