namespace Mkh.Excel.Abstractions;

/// <summary>
/// Excel模型
/// </summary>
public class ExcelModel
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 存储名称
    /// </summary>
    public string StorageName { get; set; }

    /// <summary>
    /// 存储路径
    /// </summary>
    public string StoragePath { get; set; }
}