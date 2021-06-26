using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mkh.Mod.Admin.Core.Application.Account;
using Mkh.Mod.Admin.Core.Application.Account.Dto;
using Mkh.Utils.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Mkh.Mod.Admin.Web.Controllers
{
    [SwaggerTag("账户管理")]
    public class AccountController : ModuleController
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        /// <summary>
        /// 账户列表
        /// </summary>
        /// <remarks>获取账户列表</remarks>
        [HttpGet]
        public Task<IResultModel> Index(AccountAddDto dto)
        {
            return _service.Add(dto);
        }
    }
}
