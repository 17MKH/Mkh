using System.Threading.Tasks;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.DictItem.Dto;
using Mkh.Mod.Admin.Core.Domain.DictItem;

namespace Mkh.Mod.Admin.Core.Application.DictItem;

/// <summary>
/// 数据字典项服务
/// </summary>
public interface IDictItemService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<PagingQueryResultModel<DictItemEntity>> Query(DictItemQueryDto dto);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<IResultModel> Add(DictItemAddDto dto);
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
    Task<IResultModel> Update(DictItemUpdateDto dto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResultModel> Delete(int id);
}