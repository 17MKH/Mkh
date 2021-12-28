using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Mkh.Utils.Web.FileUpload;

/// <summary>
/// 单文件上传模型
/// </summary>
public class FileUploadModel
{
    /// <summary>
    /// 文件
    /// </summary>
    public IFormFile FormFile { get; set; }

    /// <summary>
    /// 自定义文件名称
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 存储根目录
    /// </summary>
    public string RootDirectory { get; set; }

    /// <summary>
    /// 最大允许大小(单位：字节，为0表示不限制)
    /// </summary>
    public long MaxSize { get; set; }

    /// <summary>
    /// 限制文件后缀名(不包含.)
    /// </summary>
    public List<string> LimitExtensions { get; set; }

    /// <summary>
    /// 是否计算文件的MD5值
    /// </summary>
    public bool CalculateMd5 { get; set; }
}