using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Events;

namespace Mkh.Data.Core.Repository;

/// <summary>
/// 删除
/// </summary>
public abstract partial class RepositoryAbstract<TEntity>
{
    public Task<bool> Delete(dynamic id, IUnitOfWork uow = null)
    {
        return Delete(id, null, uow);
    }

    /// <summary>
    /// 删除实体，自定义表名称
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <param name="tableName">自定义表名称</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    public async Task<bool> Delete(dynamic id, string tableName, IUnitOfWork uow = null)
    {
        var dynParams = GetIdParameter(id);
        var sql = _sql.GetDeleteSingle(tableName);
        var result = await Execute(sql, dynParams, uow) > 0;

        if (result)
        {
            await HandleEntityDeleteEvent(id, tableName, uow);
        }

        return result;
    }

    /// <summary>
    /// 处理实体删除事件
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tableName"></param>
    /// <param name="uow"></param>
    /// <returns></returns>
    private async Task HandleEntityDeleteEvent(dynamic id, string tableName, IUnitOfWork uow)
    {
        if (!EntityDescriptor.EnableDeleteEvent) return;

        var events = _sp.GetServices<IEntityDeleteEvent>().ToList();
        if (!events.Any()) return;

        try
        {
            foreach (var changeEvents in events)
            {
                await changeEvents.OnDelete(new EntityDeleteContext
                {
                    DbContext = DbContext,
                    EntityDescriptor = EntityDescriptor,
                    Id = id,
                    TableName = tableName,
                    Uow = uow,
                    DeleteTime = DateTime.Now,
                    TenantId = DbContext.AccountResolver.TenantId,
                    Operator = DbContext.AccountResolver.AccountId,
                    OperatorName = DbContext.AccountResolver.AccountName
                });
            }
        }
        catch (Exception ex)
        {
            _logger.Write("HandleEntityDeleteEvent", ex.Message);
        }
    }
}