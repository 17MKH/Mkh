using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.Dict;
using Mkh.Mod.Admin.Core.Application.Dict.Dto;
using Mkh.Mod.Admin.Core.Domain.Dict;
using Swashbuckle.AspNetCore.Annotations;

namespace Mkh.Mod.Admin.Web.Controllers;

[SwaggerTag("数据字典")]
public class DictController : Web.ModuleController
{
    private readonly IDictService _service;

    public DictController(IDictService service)
    {
        _service = service;
    }

    /// <summary>
    /// 查询
    /// </summary>
    [HttpGet]
    public Task<PagingQueryResultModel<DictEntity>> Query([FromQuery] DictQueryDto dto)
    {
        return _service.Query(dto);
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <remarks></remarks>
    [HttpPost]
    public Task<IResultModel> Add(DictAddDto dto)
    {
        return _service.Add(dto);
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id"></param>
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
    public Task<IResultModel> Update(DictUpdateDto dto)
    {
        return _service.Update(dto);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public Task<IResultModel> Delete([BindRequired] int id)
    {
        return _service.Delete(id);
    }

    /// <summary>
    /// 字典下拉列表
    /// </summary>
    /// <param name="groupCode">字典分组编码</param>
    /// <param name="dictCode">字典编码</param>
    /// <returns></returns>
    [AllowWhenAuthenticated]
    [HttpGet]
    public Task<IResultModel> Select([BindRequired] string groupCode, [BindRequired] string dictCode)
    {
        return _service.Select(groupCode, dictCode);
    }

    /// <summary>
    /// 字典树
    /// </summary>
    /// <param name="groupCode">字典分组编码</param>
    /// <param name="dictCode">字典编码</param>
    /// <returns></returns>
    [AllowWhenAuthenticated]
    [HttpGet]
    public Task<IResultModel> Tree([BindRequired] string groupCode, [BindRequired] string dictCode)
    {
        return _service.Tree(groupCode, dictCode);
    }

    /// <summary>
    /// 字典级联列表
    /// </summary>
    /// <param name="groupCode">字典分组编码</param>
    /// <param name="dictCode">字典编码</param>
    /// <returns></returns>
    [AllowWhenAuthenticated]
    [HttpGet]
    public Task<IResultModel> Cascader([BindRequired] string groupCode, [BindRequired] string dictCode)
    {
        return _service.Cascader(groupCode, dictCode);
    }
}