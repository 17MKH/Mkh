namespace Mkh.Data.Abstractions;

/// <summary>
/// 代码优先提供器
/// </summary>
public interface ICodeFirstProvider
{
    /// <summary>
    /// 创建库
    /// </summary>
    void CreateDatabase();

    /// <summary>
    /// 创建表
    /// </summary>
    void CreateTable();

    /// <summary>
    /// 创建表
    /// </summary>
    void CreateNextTable();
}