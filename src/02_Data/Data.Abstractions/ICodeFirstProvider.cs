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
    /// 根据实体创建表
    /// </summary>
    /// <param name="entity">可选，要创建表的实体</param>
    void CreateTable(IEntity entity = null);

    /// <summary>
    /// 创建表
    /// </summary>
    void CreateNextTable();
}
