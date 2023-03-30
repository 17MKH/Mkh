using System;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;

namespace Mkh.Data.Core.Repository;

/// <summary>
/// 是否存在
/// </summary>
public abstract partial class RepositoryAbstract<TEntity>
{
    public async Task<bool> Exists(dynamic id, string tableName, IUnitOfWork uow = null)
    {
        //没有主键的表无法使用Exists方法
        if (EntityDescriptor.PrimaryKey.IsNo)
            throw new ArgumentException("该实体没有主键，无法使用Exists方法~");

        var dynParams = GetIdParameter(id);
        var sql = _sql.GetExists(tableName);

        _logger.Write("Exists", sql);
        return await QuerySingleOrDefault<int>(sql, dynParams, uow) > 0;
    }

    public Task<bool> Exists(dynamic id, IUnitOfWork uow = null)
    {
        return Exists(id, null, uow);
    }
}