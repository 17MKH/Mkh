using Microsoft.AspNetCore.Mvc;
using Mkh.Module.Abstractions;
using Mkh.Utils.Result;
using Swashbuckle.AspNetCore.Annotations;

namespace Mkh.Mod.Admin.Web.Controllers
{
    /// <summary>
    /// 账户管理
    /// </summary>
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
            return Ok(_modules);
        }
    }

    public class Test
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
    }
}
