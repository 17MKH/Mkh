namespace Mkh.Excel.Abstractions;

/// <summary>
/// Excel操作配置项
/// </summary>
public class ExcelOptions
{
    /// <summary>
    /// 提供程序
    /// </summary>
    public string Provider { get; set; }

    /// <summary>
    /// Excel操作时产生的临时文件存储根路径
    /// </summary>
    public string TempDir { get; set; }
}