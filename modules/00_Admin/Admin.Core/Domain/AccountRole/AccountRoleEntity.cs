using System;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.AccountRole
{
    /// <summary>
    /// 账户角色实体
    /// </summary>
    public class AccountRoleEntity : EntityBase
    {
        /// <summary>
        /// 账户编号
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public int RoleId { get; set; }
    }
}
