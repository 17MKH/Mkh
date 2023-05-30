using Mkh.Data.Abstractions.Query;

namespace Mkh.Mod.Admin.Core.Application.Dict.Dto;

public class DictQueryDto : QueryDto
{
    /// <summary>
    /// 分组编码
    /// </summary>
    public string GroupCode { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }
}