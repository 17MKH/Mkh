using System.ComponentModel;

namespace Mkh.Module.Admin.Domain.Account
{
    public enum AccountType
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin,
        /// <summary>
        /// 普通用户
        /// </summary>
        [Description("普通用户")]
        User
    }
}
