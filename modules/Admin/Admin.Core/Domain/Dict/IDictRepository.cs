using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Query;
using Mkh.Mod.Admin.Core.Application.Dict.Dto;

namespace Mkh.Mod.Admin.Core.Domain.Dict;

/// <summary>
/// 数据字典仓储
/// </summary>
public interface IDictRepository : IRepository<DictEntity>
{
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<PagingQueryResultModel<DictEntity>> Query(DictQueryDto dto);
}