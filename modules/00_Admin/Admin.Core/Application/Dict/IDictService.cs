using System.Collections.Generic;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.Dict.Dto;
using Mkh.Mod.Admin.Core.Application.Dict.Vo;
using Mkh.Mod.Admin.Core.Domain.Dict;

namespace Mkh.Mod.Admin.Core.Application.Dict;

/// <summary>
/// 字典服务
/// </summary>
public interface IDictService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<PagingQueryResultModel<DictEntity>> Query(DictQueryDto dto);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<int> Add(DictAddDto dto);

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<DictUpdateDto> Edit(int id);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task Update(DictUpdateDto dto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(int id);

    /// <summary>
    /// 查询字典下拉列表
    /// </summary>
    /// <param name="groupCode"></param>
    /// <param name="dictCode"></param>
    /// <returns></returns>
    Task<List<OptionResultModel>> Select(string groupCode, string dictCode);

    /// <summary>
    /// 查询字典树
    /// </summary>
    /// <param name="groupCode"></param>
    /// <param name="dictCode"></param>
    /// <returns></returns>
    Task<List<TreeResultModel<int, DictItemTreeVo>>> Tree(string groupCode, string dictCode);

    /// <summary>
    /// 查询字典级联列表
    /// </summary>
    /// <param name="groupCode"></param>
    /// <param name="dictCode"></param>
    /// <returns></returns>
    Task<List<OptionResultModel>> Cascader(string groupCode, string dictCode);
}