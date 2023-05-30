using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Events;

namespace Mkh.Data.Core.Repository;

/// <summary>
/// 实体更新
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract partial class RepositoryAbstract<TEntity>
{
    public Task<bool> Update(TEntity entity, IUnitOfWork uow = null)
    {
        return Update(entity, null, uow);
    }

    public async Task<bool> Update(TEntity entity, string tableName, IUnitOfWork uow = null)
    {
        Check.NotNull(entity, nameof(entity));

        SetUpdateInfo(entity);

        TEntity oldEntity = default;
        List<IEntityUpdateEvent> events = null;
        if (EntityDescriptor.EnableUpdateEvent && !EntityDescriptor.PrimaryKey.IsNo)
        {
            events = _sp.GetServices<IEntityUpdateEvent>().ToList();
        }

        if (events.NotNullAndEmpty())
        {
            oldEntity = await Get(EntityDescriptor.PrimaryKey.PropertyInfo.GetValue(entity), tableName, uow);
        }

        var sql = _sql.GetUpdateSingle(tableName);

        if (await Execute(sql, entity, uow) > 0)
        {
            if (events.NotNullAndEmpty())
            {
                try
                {
                    foreach (var changeEvents in events)
                    {
                        await changeEvents.OnUpdate(new EntityUpdateContext
                        {
                            DbContext = DbContext,
                            EntityDescriptor = EntityDescriptor,
                            NewEntity = entity,
                            OldEntity = oldEntity,
                            TableName = tableName,
                            Uow = uow,
                            UpdateTime = DateTime.Now,
                            TenantId = DbContext.AccountResolver.TenantId,
                            Operator = DbContext.AccountResolver.AccountId,
                            OperatorName = DbContext.AccountResolver.AccountName
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.Write("HandleEntityUpdateEvents", ex.Message);
                }
            }

            return true;
        }
        return false;
    }

    /// <summary>
    /// 设置更新信息
    /// </summary>
    private void SetUpdateInfo(TEntity entity)
    {
        //设置实体的修改人编号、修改人姓名、修改时间
        var descriptor = EntityDescriptor;
        if (descriptor.IsEntityBase)
        {
            foreach (var column in descriptor.Columns)
            {
                var colName = column.PropertyInfo.Name;
                if (colName.Equals("ModifiedBy"))
                {
                    var modifiedBy = column.PropertyInfo.GetValue(entity);
                    if (modifiedBy == null || (Guid)modifiedBy == Guid.Empty)
                    {
                        column.PropertyInfo.SetValue(entity, DbContext.AccountResolver.AccountId);
                    }
                    continue;
                }
                if (colName.Equals("Modifier"))
                {
                    var modifier = column.PropertyInfo.GetValue(entity);
                    if (modifier == null)
                    {
                        column.PropertyInfo.SetValue(entity, DbContext.AccountResolver.AccountName);
                    }
                    continue;
                }
                if (colName.Equals("ModifiedTime"))
                {
                    var modifiedTime = column.PropertyInfo.GetValue(entity);
                    if (modifiedTime == null)
                    {
                        column.PropertyInfo.SetValue(entity, DateTime.Now);
                    }
                }
            }
        }
    }
}