using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Events;

namespace Mkh.Data.Core.Repository;

public abstract partial class RepositoryAbstract<TEntity>
{
    public Task<bool> SoftDelete(dynamic id, IUnitOfWork uow = null)
    {
        return SoftDelete(id, null, uow);
    }

    public async Task<bool> SoftDelete(dynamic id, string tableName, IUnitOfWork uow = null)
    {
        if (!EntityDescriptor.IsSoftDelete)
            throw new Exception("该实体未继承软删除接口，无法使用软删除功能~");

        PrimaryKeyValidate(id);

        var dynParams = new DynamicParameters();
        dynParams.Add(_adapter.AppendParameter("Id"), id);
        dynParams.Add(_adapter.AppendParameter("DeletedTime"), DateTime.Now);
        dynParams.Add(_adapter.AppendParameter("DeletedBy"), DbContext.AccountResolver.AccountId);
        dynParams.Add(_adapter.AppendParameter("Deleter"), DbContext.AccountResolver.AccountName);

        var sql = _sql.GetSoftDeleteSingle(tableName);

        _logger.Write("SoftDelete", sql);

        if (await Execute(sql, dynParams, uow) > 0)
        {
            await HandleSoftDeleteEvent(id, tableName, uow);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 处理软删除事件
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tableName"></param>
    /// <param name="uow"></param>
    /// <returns></returns>
    private async Task HandleSoftDeleteEvent(dynamic id, string tableName, IUnitOfWork uow)
    {
        if (!EntityDescriptor.EnableSoftDeleteEvent) return;

        var events = _sp.GetServices<IEntitySoftDeleteEvent>().ToList();
        if (!events.Any()) return;

        try
        {
            foreach (var changeEvents in events)
            {
                await changeEvents.OnSoftDelete(new EntitySoftDeleteContext
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
            _logger.Write("HandleSoftDeleteEvent", ex.Message);
        }
    }
}