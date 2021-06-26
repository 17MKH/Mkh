using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Authorize.Vo;
using Mkh.Mod.Admin.Core.Domain.Account;

namespace Mkh.Mod.Admin.Core.Infrastructure
{
    /// <summary>
    /// 账户资料解析器
    /// </summary>
    public interface IAccountProfileResolver
    {
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<ProfileVo> Resolve(AccountEntity account);
    }
}
