using System.Collections.Generic;

namespace Mkh.Data.Abstractions.Schema;

/// <summary>
/// 数据库架构提供器接口
/// </summary>
public interface ISchemaProvider
{
    /// <summary>
    /// 是否存在指定数据库
    /// </summary>
    /// <param name="database">数据库名称</param>
    /// <returns></returns>
    bool IsExistsDatabase(string database);

    /// <summary>
    /// 是否存在指定表
    /// </summary>
    /// <param name="database">数据库名称</param>
    /// <param name="table">表名称</param>
    /// <returns></returns>
    bool IsExistsTable(string database, string table);

    /// <summary>
    /// 获取数据库架构集合，包括表架构和列架构信息
    /// </summary>
    /// <returns></returns>
    List<DatabaseSchema> GetDatabases();

    /// <summary>
    /// 根据指定数据库名称获取数据库架构信息，包括表架构和列架构信息
    /// </summary>
    /// <param name="database">数据库名称</param>
    /// <returns></returns>
    DatabaseSchema GetDatabase(string database);

    /// <summary>
    /// 获取指定数据库的表架构集合，包括列架构信息
    /// </summary>
    /// <param name="database">数据库名称</param>
    /// <returns></returns>
    List<TableSchema> GetTables(string database);

    /// <summary>
    /// 获取指定数据库和表的列架构信息
    /// </summary>
    /// <param name="database"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    List<ColumnSchema> GetColumns(string database, string table);

    /// <summary>
    /// 创建数据库
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    void CreateDatabase(string database);
}