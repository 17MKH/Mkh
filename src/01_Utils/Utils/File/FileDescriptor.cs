using System.IO;
using System.Text.Json.Serialization;

namespace Mkh.Utils.File;

/// <summary>
/// 文件描述符
/// </summary>
public class FileDescriptor
{
    public FileDescriptor() { }

    /// <summary>
    /// 初始化文件信息
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="size">大小</param>
    public FileDescriptor(string name, long size = 0L)
    {
        Check.NotNull(name, nameof(name), "文件名称不能为空");

        Name = name;
        Size = new FileSize(size);
        Extension = System.IO.Path.GetExtension(Name)?.TrimStart('.');
    }

    /// <summary>
    /// 原始文件名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 文件存储名称
    /// </summary>
    public string StorageName { get; set; }

    /// <summary>
    /// 文件大小
    /// </summary>
    public FileSize Size { get; set; }

    /// <summary>
    /// 扩展名
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// 文件的MD5值
    /// </summary>
    public string Md5 { get; set; }

    /// <summary>
    /// 访问地址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 根目录
    /// </summary>
    [JsonIgnore]
    public string RootDirectory { get; set; }

    /// <summary>
    /// 完整目录
    /// </summary>
    [JsonIgnore]
    public string FullDirectory => Path.Combine(RootDirectory, RelativeDirectory);

    /// <summary>
    /// 相对目录
    /// </summary>
    [JsonIgnore]
    public string RelativeDirectory { get; set; }

    /// <summary>
    /// 文件的完整路径名称
    /// </summary>
    [JsonIgnore]
    public string FullName => Path.Combine(RootDirectory, RelativeDirectory, StorageName);

    /// <summary>
    /// 文件的相对完整路径名称
    /// </summary>
    [JsonIgnore]
    public string RelativeFullName => Path.Combine(RelativeDirectory, StorageName);
}