using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mkh.Excel.Abstractions;

/// <summary>
/// Excel导出结果模型
/// </summary>
public class ExcelExportResultModel
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 存储名称
    /// </summary>
    public string SaveName { get; set; }

    /// <summary>
    /// 存储路径
    /// </summary>
    public string Path { get; set; }
}