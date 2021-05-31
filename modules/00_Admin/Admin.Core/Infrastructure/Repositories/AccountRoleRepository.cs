using System;
using System.Threading.Tasks;
using Mkh.Data.Core.Repository;
using Mkh.Mod.Admin.Core.Domain.AccountRole;

namespace Mkh.Mod.Admin.Core.Infrastructure.Repositories
{
    public class AccountRoleRepository : RepositoryAbstract<AccountRoleEntity>, IAccountRoleRepository
    {
        public Task<bool> Exists(Guid accountId, string roleCode)
        {
            throw new NotImplementedException();
        }
    }
}
