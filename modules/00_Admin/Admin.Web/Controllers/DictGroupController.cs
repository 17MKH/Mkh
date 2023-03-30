using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Mod.Admin.Core.Application.DictGroup;
using Mkh.Mod.Admin.Core.Application.DictGroup.Dto;
using Mkh.Mod.Admin.Core.Domain.DictGroup;

namespace Mkh.Mod.Admin.Web.Controllers;

[Tags("数据字典分组")]
public class DictGroupController : Web.ModuleController
{
    private readonly IDictGroupService _service;

    public DictGroupController(IDictGroupService service)
    {
        _service = service;
    }

    /// <summary>
    /// 查询
    /// </summary>
    [HttpGet]
    public Task<IResultModel<IList<DictGroupEntity>>> Query([FromQuery] DictGroupQueryDto dto)
    {
        return _service.Query(dto);
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel<int>> Add(DictGroupAddDto dto)
    {
        return Success(_service.Add(dto));
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResultModel<DictGroupUpdateDto>> Edit(int id)
    {
        return Success(_service.Edit(id));
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel> Update(DictGroupUpdateDto dto)
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
