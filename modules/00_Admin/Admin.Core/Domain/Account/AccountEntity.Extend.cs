using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Domain.Account
{
    public partial class AccountEntity
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [Ignore]
        public string TenantName { get; set; }
    }
}
