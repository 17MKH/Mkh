using System.Threading.Tasks;
using Mkh.Mod.Admin.Core.Application.Account.Dto;
using Mkh.Utils.Models;

namespace Mkh.Mod.Admin.Core.Application.Account
{
    public interface IAccountService
    {
        /// <summary>
        /// 添加账户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IResultModel> Add(AccountAddDto dto);
    }
}
