namespace Mkh.Utils.File;

/// <summary>
/// 文件大小
/// </summary>
public readonly struct FileSize
{
    /// <summary>
    /// 初始化文件大小
    /// </summary>
    /// <param name="size">文件大小</param>
    /// <param name="unit">文件大小单位</param>
    public FileSize(long size, FileSizeUnit unit = FileSizeUnit.Byte)
    {
        switch (unit)
        {
            case FileSizeUnit.K:
                Length = size * 1024; break;
            case FileSizeUnit.M:
                Length = size * 1024 * 1024; break;
            case FileSizeUnit.G:
                Length = size * 1024 * 1024 * 1024; break;
            default:
                Length = size; break;
        }
    }

    /// <summary>
    /// 文件字节长度
    /// </summary>
    public long Length { get; }

    /// <summary>
    /// 获取文件大小，单位：字节
    /// </summary>
    public long GetSize()
    {
        return Length;
    }

    /// <summary>
    /// 获取文件大小，单位：K
    /// </summary>
    public double GetSizeByK()
    {
        return (Length / 1024.0).ToDouble(2);
    }

    /// <summary>
    /// 获取文件大小，单位：M
    /// </summary>
    public double GetSizeByM()
    {
        return (Length / 1024.0 / 1024.0).ToDouble(2);
    }

    /// <summary>
    /// 获取文件大小，单位：G
    /// </summary>
    public double GetSizeByG()
    {
        return (Length / 1024.0 / 1024.0 / 1024.0).ToDouble(2);
    }

    /// <summary>
    /// 输出描述
    /// </summary>
    public override string ToString()
    {
        if (Length >= 1024 * 1024 * 1024)
            return $"{GetSizeByG()} {FileSizeUnit.G.ToDescription()}";
        if (Length >= 1024 * 1024)
            return $"{GetSizeByM()} {FileSizeUnit.M.ToDescription()}";
        if (Length >= 1024)
            return $"{GetSizeByK()} {FileSizeUnit.K.ToDescription()}";
        return $"{Length} {FileSizeUnit.Byte.ToDescription()}";
    }
}