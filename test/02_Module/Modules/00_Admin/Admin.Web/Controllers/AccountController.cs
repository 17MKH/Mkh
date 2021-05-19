using Microsoft.AspNetCore.Mvc;

namespace Mkh.Module.Admin.Web.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IActionResult Login()
        {
            return Ok("登录成功");
        }
    }
}
