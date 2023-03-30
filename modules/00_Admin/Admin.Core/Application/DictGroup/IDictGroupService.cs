﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.DictGroup.Dto;
using Mkh.Mod.Admin.Core.Domain.DictGroup;

namespace Mkh.Mod.Admin.Core.Application.DictGroup;

/// <summary>
/// 数据字典服务
/// </summary>
public interface IDictGroupService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<IResultModel<IList<DictGroupEntity>>> Query(DictGroupQueryDto dto);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<int> Add(DictGroupAddDto dto);

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<DictGroupUpdateDto> Edit(int id);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task Update(DictGroupUpdateDto dto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(int id);

    /// <summary>
    /// 下拉选项
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<OptionResultModel>> Select();
}
