namespace Mkh.Utils.Models;

/// <summary>
/// 文件下载模型
/// </summary>
public class FileDownloadModel
{
    /// <summary>
    /// 文件完整路径
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 文件内容类型
    /// </summary>
    public string ContentType { get; set; }
}