using Microsoft.AspNetCore.Mvc;
using Mkh.Module.Abstractions;
using Mkh.Utils.Result;
using Swashbuckle.AspNetCore.Annotations;

namespace Mkh.Mod.Admin.Web.Controllers
{
    [SwaggerTag("账户管理")]
    public class AccountController : ModuleController
    {
        private readonly IModuleCollection _modules;

        public AccountController(IModuleCollection modules)
        {
            _modules = modules;
        }

        /// <summary>
        /// 账户列表
        /// </summary>
        /// <remarks>获取账户列表</remarks>
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(ResultModel.Success(_modules));
        }
    }
}
