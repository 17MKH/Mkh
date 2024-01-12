using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mkh.Excel.Abstractions;
using Mkh.Utils;
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
    /// 成功
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    protected Task<IResultModel> Success(Task task)
    {
        return Result.SuccessAsync(task);
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task"></param>
    /// <returns></returns>
    protected Task<IResultModel<T>> Success<T>(Task<T> task)
    {
        return Result.SuccessAsync(task);
    }

    /// <summary>
    /// 文件下载
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    protected IActionResult FileDownload(FileDownloadModel model)
    {
        return PhysicalFile(model.FilePath, model.ContentType ?? "application/octet-stream", model.FileName, true);
    }

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    protected IActionResult ExportExcel(ExcelModel model)
    {
        if (model.FileName.IsNull())
        {
            model.FileName = DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        return PhysicalFile(model.StoragePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", model.FileName, true);
    }
}