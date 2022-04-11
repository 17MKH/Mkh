using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Core.Descriptors;

/// <summary>
/// 实体的SQL语句
/// </summary>
internal class EntitySqlDescriptor : IEntitySqlDescriptor
{
    #region ==字段==

    /// <summary>
    /// 数据库适配器
    /// </summary>
    private readonly IDbAdapter _adapter;

    /// <summary>
    /// 表名称
    /// </summary>
    private readonly string _tableName;

    #endregion

    #region ==构造函数==

    public EntitySqlDescriptor(IDbAdapter adapter, string tableName)
    {
        _adapter = adapter;
        _tableName = tableName;
    }

    #endregion

    #region ==方法==

    #region ==添加==

    /// <summary>
    /// 添加语句
    /// </summary>
    private string _add;
    private string _defaultAdd;

    /// <summary>
    /// 设置新增语句
    /// </summary>
    /// <param name="sql"></param>
    public void SetAdd(string sql)
    {
        _add = sql;
        _defaultAdd = FormatSql(sql);
    }

    /// <summary>
    /// 获取新增语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetAdd(string tableName)
    {
        return tableName.IsNull() ? _defaultAdd : FormatSql(_add, tableName);
    }

    #endregion

    #region ==批量添加==

    /// <summary>
    /// 批量新增语句
    /// </summary>
    private string _bulkAdd;
    private string _defaultBulkAdd;

    /// <summary>
    /// 设置批量新增语句
    /// </summary>
    /// <param name="sql"></param>
    public void SetBulkAdd(string sql)
    {
        _bulkAdd = sql;
        _defaultBulkAdd = FormatSql(sql);
    }

    /// <summary>
    /// 获取批量插入语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetBulkAdd(string tableName)
    {
        return tableName.IsNull() ? _defaultBulkAdd : FormatSql(_bulkAdd, tableName);
    }

    #endregion

    #region ==删除单条==

    /// <summary>
    /// 删除单条语句
    /// </summary>
    private string _deleteSingle;
    private string _defaultDeleteSingle;

    /// <summary>
    /// 设置删除单挑SQL语句
    /// </summary>
    /// <param name="sql"></param>
    public void SetDeleteSingle(string sql)
    {
        _deleteSingle = sql;
        _defaultDeleteSingle = FormatSql(sql);
    }

    /// <summary>
    /// 获取单条删除语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetDeleteSingle(string tableName)
    {
        return tableName.IsNull() ? _defaultDeleteSingle : FormatSql(_deleteSingle, tableName);
    }

    #endregion

    #region ==条件删除==

    /// <summary>
    /// 删除语句
    /// </summary>
    private string _delete;
    private string _defaultDelete;

    /// <summary>
    /// 设置条件删除语句
    /// </summary>
    /// <param name="sql"></param>
    public void SetDelete(string sql)
    {
        _delete = sql;
        _defaultDelete = FormatSql(_delete);
    }

    /// <summary>
    /// 获取条件删除语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetDelete(string tableName)
    {
        return tableName.IsNull() ? _defaultDelete : FormatSql(_delete, tableName);
    }

    #endregion

    #region ==软删除单条==

    /// <summary>
    /// 软删除单条数据
    /// </summary>
    private string _softDeleteSingle;
    private string _defaultSoftDeleteSingle;

    /// <summary>
    /// 设置单挑软删除语句
    /// </summary>
    /// <param name="sql"></param>
    public void SetSoftDeleteSingle(string sql)
    {
        _softDeleteSingle = sql;
        _defaultSoftDeleteSingle = FormatSql(sql);
    }

    /// <summary>
    /// 获取软删除单条语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetSoftDeleteSingle(string tableName)
    {
        return tableName.IsNull() ? _defaultSoftDeleteSingle : FormatSql(_softDeleteSingle, tableName);
    }

    #endregion

    #region ==软删除批量==

    /// <summary>
    /// 软删除语句
    /// </summary>
    private string _softDelete;
    private string _defaultSoftDelete;

    /// <summary>
    /// 设置批量软删除
    /// </summary>
    /// <param name="sql"></param>
    public void SetSoftDelete(string sql)
    {
        _softDelete = sql;
        _defaultSoftDelete = FormatSql(_softDelete);
    }

    /// <summary>
    /// 获取批量软删除
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetSoftDelete(string tableName)
    {
        return tableName.IsNull() ? _defaultSoftDelete : FormatSql(_softDelete, tableName);
    }

    #endregion

    #region ==更新单个实体==

    /// <summary>
    /// 修改单条语句
    /// </summary>
    private string _updateSingle;
    private string _defaultUpdateSingle;

    /// <summary>
    /// 设置更新实体语句
    /// </summary>
    /// <param name="sql"></param>
    public void SetUpdateSingle(string sql)
    {
        _updateSingle = sql;
        _defaultUpdateSingle = FormatSql(sql);
    }

    /// <summary>
    /// 获取更新实体语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetUpdateSingle(string tableName)
    {
        return tableName.IsNull() ? _defaultUpdateSingle : FormatSql(_updateSingle, tableName);
    }

    #endregion

    #region ==条件更新==

    /// <summary>
    /// 修改语句
    /// </summary>
    private string _update;
    private string _defaultUpdate;

    /// <summary>
    /// 设置条件更新实体语句
    /// </summary>
    /// <param name="sql"></param>
    public void SetUpdate(string sql)
    {
        _update = sql;
        _defaultUpdate = FormatSql(sql);
    }

    /// <summary>
    /// 获取条件更新实体语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetUpdate(string tableName)
    {
        return tableName.IsNull() ? _defaultUpdate : FormatSql(_update, tableName);
    }

    #endregion

    #region ==查询单个实体==

    /// <summary>
    /// 单条语句
    /// </summary>
    private string _get;
    private string _defaultGet;

    /// <summary>
    /// 设置查询单条语句
    /// </summary>
    /// <param name="sql"></param>
    public void SetGet(string sql)
    {
        _get = sql;
        _defaultGet = FormatSql(sql);
    }

    /// <summary>
    /// 获取单个实体语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetGet(string tableName)
    {
        return tableName.IsNull() ? _defaultGet : FormatSql(_get, tableName);
    }

    #endregion

    #region ==查询单个实体带锁==

    /// <summary>
    /// 查询单条语句并行锁
    /// </summary>
    private string _getAndRowLock;
    private string _defaultGetAndRowLock;

    /// <summary>
    /// 设置获取单个实体语句(行锁)
    /// </summary>
    /// <param name="sql"></param>
    public void SetGetAndRowLock(string sql)
    {
        _getAndRowLock = sql;
        _defaultGetAndRowLock = FormatSql(sql);
    }

    /// <summary>
    /// 获取单个实体语句(行锁)
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetGetAndRowLock(string tableName)
    {
        return tableName.IsNull() ? _defaultGetAndRowLock : FormatSql(_getAndRowLock, tableName);
    }

    #endregion

    #region ==查询单个实体无锁==

    /// <summary>
    /// 查询单条语句并排除锁(仅SqlServer可用)
    /// </summary>
    private string _getAndNoLock;
    private string _defaultGetAndNoLock;

    /// <summary>
    /// 设置单个实体语句(无锁)
    /// </summary>
    /// <param name="sql"></param>
    public void SetGetAndNoLock(string sql)
    {
        _getAndNoLock = sql;
        _defaultGetAndNoLock = FormatSql(sql);
    }

    /// <summary>
    /// 获取单个实体语句(无锁)
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetGetAndNoLock(string tableName)
    {
        return tableName.IsNull() ? _defaultGetAndNoLock : FormatSql(_getAndNoLock, tableName);
    }

    #endregion

    #region ==查询语句==

    /// <summary>
    /// 查询语句
    /// </summary>
    private string _query;
    private string _defaultQuery;

    /// <summary>
    /// 设置查询语句
    /// </summary>
    /// <param name="sql"></param>
    public void SetQuery(string sql)
    {
        _query = sql;
        _defaultQuery = FormatSql(sql);
    }

    /// <summary>
    /// 获取查询语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetQuery(string tableName)
    {
        return tableName.IsNull() ? _defaultQuery : FormatSql(_query, tableName);
    }

    #endregion

    #region ==存在语句==

    /// <summary>
    /// 是否存在语句
    /// </summary>
    private string _exists;
    private string _defaultExists;

    /// <summary>
    /// 设置存在语句
    /// </summary>
    /// <param name="sql"></param>
    public void SetExists(string sql)
    {
        _exists = sql;
        _defaultExists = FormatSql(sql);
    }

    /// <summary>
    /// 获取存在语句
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetExists(string tableName)
    {
        return tableName.IsNull() ? _defaultExists : FormatSql(_exists, tableName);
    }

    #endregion

    #endregion

    #region ==私有方法==

    /// <summary>
    /// 格式化SQL
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="tableName">表名称</param>
    /// <returns></returns>
    private string FormatSql(string sql, string tableName = null)
    {
        return string.Format(sql, _adapter.AppendQuote(tableName ?? _tableName));
    }

    #endregion
}