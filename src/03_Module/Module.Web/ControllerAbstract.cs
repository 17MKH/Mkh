using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mkh.Utils.Models;

namespace Mkh.Module.Web;

/// <summary>
/// 控制器抽象
/// </summary>
[Route("api/[area]/[controller]/[action]")]
[ApiController]
[Authorize(Policy = "MKH")]
[ValidateResultFormat]
public abstract class ControllerAbstract : ControllerBase
{
    /// <summary>
    /// 文件下载
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    protected IActionResult FileDownload(FileDownloadModel model)
    {
        return PhysicalFile(model.FilePath, model.ContentType ?? "application/octet-stream", model.FileName, true);
    }
}