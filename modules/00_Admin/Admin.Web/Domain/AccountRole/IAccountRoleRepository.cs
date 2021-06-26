using System;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;

namespace Mkh.Mod.Admin.Core.Domain.AccountRole
{
    /// <summary>
    /// 账户角色关联仓储
    /// </summary>
    public interface IAccountRoleRepository : IRepository<AccountRoleEntity>
    {
        /// <summary>
        /// 判断绑定关系是否已存在
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        Task<bool> Exists(Guid accountId, string roleCode);
    }
}
