using System.Threading.Tasks;
using Mkh.Data.Abstractions.Query;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Repository;
using Mkh.Mod.Admin.Core.Application.Dict.Dto;
using Mkh.Mod.Admin.Core.Domain.Dict;

namespace Mkh.Mod.Admin.Core.Infrastructure.Repositories;

public class DictRepository : RepositoryAbstract<DictEntity>, IDictRepository
{
    public Task<PagingQueryResultModel<DictEntity>> Query(DictQueryDto dto)
    {
        return QueryBuilder(dto).ToPaginationResult(dto.Paging);
    }

    private IQueryable<DictEntity> QueryBuilder(DictQueryDto dto)
    {
        var query = Find();
        query.WhereNotNull(dto.GroupCode, m => m.GroupCode.Equals(dto.GroupCode));
        query.WhereNotNull(dto.Name, m => m.Name.Equals(dto.Name));
        query.WhereNotNull(dto.Code, m => m.Code.Equals(dto.Code));
        return query;
    }
}