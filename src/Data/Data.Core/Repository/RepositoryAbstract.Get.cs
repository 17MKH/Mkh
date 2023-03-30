using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Exceptions;

namespace Mkh.Data.Core.Repository;

public abstract partial class RepositoryAbstract<TEntity>
{
    public async Task<TEntity> Get(dynamic id, IUnitOfWork uow = null)
    {
        var entity = await Get(id, null, uow);
        if (entity == null)
        {
            throw new EntityNotFoundException();
        }

        return entity;
    }

    public async Task<TEntity> Get(dynamic id, string tableName, IUnitOfWork uow = null)
    {
        var entity = await Get(id, tableName, uow, false, false);
        if (entity == null)
        {
            throw new EntityNotFoundException();
        }

        return entity;
    }

    public Task<TEntity> GetOrDefault(dynamic id, IUnitOfWork uow = null)
    {
        return Get(id, null, uow, false, false);
    }

    public Task<TEntity> GetOrDefault(dynamic id, string tableName, IUnitOfWork uow = null)
    {
        return Get(id, tableName, uow, false, false);
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
    protected Task<TEntity> Get(dynamic id, string tableName, IUnitOfWork uow, bool rowLock, bool noLock = false)
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