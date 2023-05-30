using System;
using Microsoft.AspNetCore.Mvc;
using Mkh.Excel.Abstractions;
using Mkh.Module.Web;

namespace Mkh.Mod.Admin.Web;

[Area("Admin")]
public abstract class ModuleController : ControllerAbstract
{
    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
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