namespace Mkh.Data.Abstractions.Schema;

/// <summary>
/// 列结构
/// </summary>
public class ColumnSchema
{
    /// <summary>
    /// 列名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 数据类型
    /// </summary>
    public string DataType { get; set; }

    /// <summary>
    /// 是否可空的
    /// </summary>
    public bool IsNullable { get; set; }

    /// <summary>
    /// 是否主键
    /// </summary>
    public bool IsPrimaryKey { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    public string DefaultValue { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 字符长度
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// 整数位
    /// </summary>
    public int Precision { get; set; }

    /// <summary>
    /// 小数位
    /// </summary>
    public int Scale { get; set; }
}