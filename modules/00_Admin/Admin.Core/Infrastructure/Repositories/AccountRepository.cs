using System;
using System.Threading.Tasks;
using Mkh.Data.Core.Repository;
using Mkh.Mod.Admin.Core.Domain.Account;

namespace Mkh.Mod.Admin.Core.Infrastructure.Repositories;

public class AccountRepository : RepositoryAbstract<AccountEntity>, IAccountRepository
{
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

    public Task<AccountEntity> GetByUserName(string username)
    {
        return Find(m => m.Username == username).ToFirst();
    }
}