using Microsoft.AspNetCore.Mvc;
using Mkh.Auth.Abstractions.Annotations;

namespace Mkh.Module.Web
{
    /// <summary>
    /// 控制器抽象
    /// </summary>
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    [MkhAuthorize]
    public abstract class ControllerAbstract : ControllerBase
    {
        
    }
}
