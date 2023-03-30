namespace Mkh.Data.Abstractions.Descriptors;

/// <summary>
/// 实体的SQL语句
/// </summary>
public interface IEntitySqlDescriptor
{
    /// <summary>
    /// 获取新增语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetAdd(string tableName);

    /// <summary>
    /// 获取批量新增语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetBulkAdd(string tableName);

    /// <summary>
    /// 获取单条删除语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetDeleteSingle(string tableName);

    /// <summary>
    /// 获取条件删除语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetDelete(string tableName);

    /// <summary>
    /// 获取软删除单条语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetSoftDeleteSingle(string tableName);

    /// <summary>
    /// 获取批量软删除
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetSoftDelete(string tableName);

    /// <summary>
    /// 获取更新实体语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetUpdateSingle(string tableName);

    /// <summary>
    /// 获取条件更新实体语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetUpdate(string tableName);

    /// <summary>
    /// 获取单个实体语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetGet(string tableName);

    /// <summary>
    /// 获取单个实体语句(行锁)
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetGetAndRowLock(string tableName);

    /// <summary>
    /// 获取单个实体语句(无锁)
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetGetAndNoLock(string tableName);

    /// <summary>
    /// 获取查询语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetQuery(string tableName);

    /// <summary>
    /// 获取存在语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    string GetExists(string tableName);
}