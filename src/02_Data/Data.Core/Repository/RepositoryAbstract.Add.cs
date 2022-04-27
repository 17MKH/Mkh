using System;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.EntityChangeEvents;

namespace Mkh.Data.Core.Repository;

/// <summary>
/// 新增
/// </summary>
public abstract partial class RepositoryAbstract<TEntity>
{
    public Task<bool> Add(TEntity entity, IUnitOfWork uow = null)
    {
        return Add(entity, null, uow);
    }

    /// <summary>
    /// 新增，自定义表名称
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="tableName"></param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    protected async Task<bool> Add(TEntity entity, string tableName, IUnitOfWork uow = null)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "entity is null");

        var sql = _sql.GetAdd(tableName);

        SetCreateInfo(entity);

        SetTenantInfo(entity);

        _logger.Write("Add", sql);

        var primaryKey = EntityDescriptor.PrimaryKey;
        if (primaryKey.IsInt)
        {
            //自增主键
            sql += _adapter.IdentitySql;
            var id = await ExecuteScalar<int>(sql, entity, uow);
            if (id > 0)
            {
                primaryKey.PropertyInfo.SetValue(entity, id);

                _logger.Write("NewID", id.ToString());
                return true;
            }

            return false;
        }

        if (primaryKey.IsLong)
        {
            //自增主键
            sql += _adapter.IdentitySql;
            var id = await ExecuteScalar<long>(sql, entity, uow);
            if (id > 0)
            {
                primaryKey.PropertyInfo.SetValue(entity, id);

                _logger.Write("NewID", id.ToString());
                return true;
            }
            return false;
        }

        if (primaryKey.IsGuid)
        {
            var id = (Guid)primaryKey.PropertyInfo.GetValue(entity)!;
            if (id == Guid.Empty)
            {
                primaryKey.PropertyInfo.SetValue(entity, _adapter.CreateSequentialGuid());
            }

            _logger.Write("NewID", id.ToString());

            if (await Execute(sql, entity, uow) > 0)
            {
                return true;
            }
            return false;
        }

        if (await Execute(sql, entity, uow) > 0)
        {
            try
            {
                foreach (var changeEvents in DbContext.EntityChangeEvents)
                {
                    await changeEvents.OnAdd(new EntityAddEventContext
                    {
                        EntityDescriptor = EntityDescriptor,
                        Entity = entity
                    });
                }
            }
            catch
            {
                _logger.Write("EntityChangeAddEvent", "error");
            }

            return true;
        }

        return false;
    }
}