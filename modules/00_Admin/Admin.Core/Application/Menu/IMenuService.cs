using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Menu.Dto;
using Mkh.Utils.Models;

namespace Mkh.Mod.Admin.Core.Application.Menu
{
    /// <summary>
    /// 菜单服务
    /// </summary>
    public interface IMenuService
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
    }
}
