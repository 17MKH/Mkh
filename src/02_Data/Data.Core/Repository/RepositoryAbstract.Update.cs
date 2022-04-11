using System;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;

namespace Mkh.Data.Core.Repository;

/// <summary>
/// 实体更新
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract partial class RepositoryAbstract<TEntity>
{
    public Task<bool> Update(TEntity entity, IUnitOfWork uow = null)
    {
        return UpdateAsync(entity, null, uow);
    }

    protected async Task<bool> UpdateAsync(TEntity entity, string tableName, IUnitOfWork uow = null)
    {
        Check.NotNull(entity, nameof(entity));

        SetUpdateInfo(entity);

        var sql = _sql.GetUpdateSingle(tableName);

        var result = await Execute(sql, entity, uow) > 0;
        return result;
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