using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Authorize.Dto;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Utils.Models;

namespace Mkh.Mod.Admin.Core.Application.Authorize
{
    /// <summary>
    /// 认证服务
    /// </summary>
    public interface IAuthorizeService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IResultModel<AccountEntity>> Login(LoginDto dto);
    }
}
