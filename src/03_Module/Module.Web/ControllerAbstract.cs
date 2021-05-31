using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mkh.Module.Web
{
    /// <summary>
    /// 控制器抽象
    /// </summary>
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "MKH")]
    public abstract class ControllerAbstract : ControllerBase
    {
        
    }
}
