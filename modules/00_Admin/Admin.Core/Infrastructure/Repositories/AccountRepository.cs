using System;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Query;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Repository;
using Mkh.Mod.Admin.Core.Application.Account.Dto;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Domain.Role;

namespace Mkh.Mod.Admin.Core.Infrastructure.Repositories;

public class AccountRepository : RepositoryAbstract<AccountEntity>, IAccountRepository
{
    public Task<PagingQueryResultModel<AccountEntity>> Query(AccountQueryDto dto)
    {
        var query = QueryBuilder(dto);
        return query.ToPaginationResult(dto.Paging);
    }

    public Task<bool> ExistsUsername(string username, Guid? id = null)
    {
        return Find(m => m.Username == username).WhereNotNull(id, m => m.Id != id).ToExists();
    }

    public Task<bool> ExistsPhone(string phone, Guid? id = null)
    {
        return Find(m => m.Phone == phone).WhereNotNull(id, m => m.Id != id).ToExists();
    }

    public Task<bool> ExistsEmail(string email, Guid? id = null)
    {
        return Find(m => m.Email == email).WhereNotNull(id, m => m.Id != id).ToExists();
    }

    public Task<AccountEntity> GetByUserName(string username, Guid? tenantId = null)
    {
        return Find(m => m.Username == username).WhereNotNull(tenantId, m => m.TenantId == tenantId.Value).ToFirst();
    }

    private IQueryable<AccountEntity, RoleEntity> QueryBuilder(AccountQueryDto dto)
    {
        return Find()
            .WhereNotNull(dto.Username, m => m.Username == dto.Username)
            .WhereNotNull(dto.Name, m => m.Name.Contains(dto.Name))
            .WhereNotNull(dto.Phone, m => m.Phone.Contains(dto.Phone))
            .LeftJoin<RoleEntity>(m => m.T1.RoleId == m.T2.Id)
            .Select(m => new { m.T1, RoleName = m.T2.Name })
            .OrderByDescending(m => m.T1.Id);
    }
}