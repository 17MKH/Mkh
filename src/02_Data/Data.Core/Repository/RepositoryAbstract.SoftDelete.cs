using System;
using System.Threading.Tasks;
using Dapper;
using Mkh.Data.Abstractions;

namespace Mkh.Data.Core.Repository;

public abstract partial class RepositoryAbstract<TEntity>
{
    public Task<bool> SoftDelete(dynamic id, IUnitOfWork uow = null)
    {
        return SoftDelete(id, null, uow);
    }

    protected async Task<bool> SoftDelete(dynamic id, string tableName, IUnitOfWork uow = null)
    {
        if (!EntityDescriptor.IsSoftDelete)
            throw new Exception("该实体未继承软删除接口，无法使用软删除功能~");

        PrimaryKeyValidate(id);

        var dynParams = new DynamicParameters();
        dynParams.Add(_adapter.AppendParameter("Id"), id);
        dynParams.Add(_adapter.AppendParameter("DeletedTime"), DateTime.Now);
        dynParams.Add(_adapter.AppendParameter("DeletedBy"), DbContext.AccountResolver.AccountId);
        dynParams.Add(_adapter.AppendParameter("Deleter"), DbContext.AccountResolver.Username);

        var sql = _sql.GetSoftDeleteSingle(tableName);

        _logger.Write("SoftDelete", sql);

        return await Execute(sql, dynParams, uow) > 0;
    }
}