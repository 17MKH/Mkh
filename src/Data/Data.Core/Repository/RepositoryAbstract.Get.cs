using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;

namespace Mkh.Data.Core.Repository;

public abstract partial class RepositoryAbstract<TEntity>
{
    public Task<TEntity> Get(dynamic id, IUnitOfWork uow = null)
    {
        return Get(id, null, uow);
    }

    public Task<TEntity> Get(dynamic id, string tableName, IUnitOfWork uow = null)
    {
        return Get(id, tableName, uow, false, false);
    }

    public Task<TEntity> Get(Expression<Func<TEntity, bool>> expression, IUnitOfWork uow = null)
    {
        return Find(expression).UseUow(uow).ToFirst();
    }

    public Task<TEntity> Get(Expression<Func<TEntity, bool>> expression, string tableName, IUnitOfWork uow = null)
    {
        return Find(expression, tableName).UseUow(uow).ToFirst();
    }

    /// <summary>
    /// 查询单个实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <param name="tableName">自定义表名称</param>
    /// <param name="uow">工作单元</param>
    /// <param name="rowLock">行锁</param>
    /// <param name="noLock">无锁(SqlServer有效)</param>
    /// <returns></returns>
    protected Task<TEntity> Get(dynamic id, string tableName, IUnitOfWork uow = null, bool rowLock = false, bool noLock = false)
    {
        var dynParams = GetIdParameter(id);
        string sql;
        if (rowLock)
            sql = _sql.GetGetAndRowLock(tableName);
        else if (_adapter.Provider == DbProvider.SqlServer && noLock)
            sql = _sql.GetGetAndNoLock(tableName);
        else
            sql = _sql.GetGet(tableName);

        _logger.Write("Get", sql);
        return QuerySingleOrDefault<TEntity>(sql, dynParams, uow);
    }
}