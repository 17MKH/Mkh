using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Role.Dto;
using Mkh.Utils.Models;

namespace Mkh.Mod.Admin.Core.Application.Role
{
    /// <summary>
    /// 角色服务
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        Task<IResultModel> Query();

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
    }
}
