using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;

namespace Mkh.Data.Core.Repository
{
    public abstract partial class RepositoryAbstract<TEntity>
    {
        public Task<bool> BulkAdd(IList<TEntity> entities, int flushSize = 0, IUnitOfWork uow = null)
        {
            return BulkAdd(entities, null, flushSize, uow);
        }

        /// <summary>
        /// 批量增加
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="tableName">自定义表名称</param>
        /// <param name="flushSize">单词刷新数</param>
        /// <param name="uow">工作单元</param>
        /// <returns></returns>
        public async Task<bool> BulkAdd(IList<TEntity> entities, string tableName, int flushSize = 0, IUnitOfWork uow = null)
        {
            if (entities.IsNullOrEmpty())
                return false;

            if (flushSize <= 0)
            {
                flushSize = _adapter.Options.Provider == DbProvider.SqlServer ? 1000 : 10000;
            }

            //判断有没有事务
            var hasTran = true;
            uow ??= Uow;

            if (uow == null)
            {
                uow = DbContext.NewUnitOfWork();
                hasTran = false;
            }

            try
            {
                var dbProvider = _adapter.Options.Provider;
                if (dbProvider == DbProvider.Sqlite)
                {
                    #region ==SQLite使用Dapper的官方方法==

                    if (EntityDescriptor.PrimaryKey.IsGuid)
                    {
                        foreach (var entity in entities)
                        {
                            SetCreateInfo(entity);

                            SetTenantInfo(entity);

                            var value = (Guid)EntityDescriptor.PrimaryKey.PropertyInfo.GetValue(entity)!;
                            if (value == Guid.Empty)
                            {
                                value = _adapter.CreateSequentialGuid();
                                EntityDescriptor.PrimaryKey.PropertyInfo.SetValue(entity, value);
                            }
                        }

                    }

                    await Execute(_sql.GetAdd(tableName), entities);

                    _logger.Write("BulkAdd", entities.Count.ToString());

                    #endregion
                }
                else
                {
                    #region ==自定义==

                    var sqlBuilder = new StringBuilder();

                    for (var t = 0; t < entities.Count; t++)
                    {
                        var mod = (t + 1) % flushSize;
                        if (mod == 1)
                        {
                            sqlBuilder.Clear();
                            sqlBuilder.Append(_sql.GetBulkAdd(tableName));
                        }

                        var entity = entities[t];

                        SetCreateInfo(entity);
                        SetTenantInfo(entity);

                        sqlBuilder.Append("(");
                        for (var i = 0; i < EntityDescriptor.Columns.Count; i++)
                        {
                            var col = EntityDescriptor.Columns[i];
                            var value = col.PropertyInfo.GetValue(entity);
                            var type = col.PropertyInfo.PropertyType;

                            if (col.IsPrimaryKey)
                            {
                                if (EntityDescriptor.PrimaryKey.IsGuid)
                                {
                                    if ((Guid)value! == Guid.Empty)
                                    {
                                        value = _adapter.CreateSequentialGuid();
                                        EntityDescriptor.PrimaryKey.PropertyInfo.SetValue(entity, value);
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            AppendValue(sqlBuilder, type, value);

                            if (i < EntityDescriptor.Columns.Count - 1)
                                sqlBuilder.Append(",");
                        }

                        sqlBuilder.Append(")");

                        if (mod > 0 && t < entities.Count - 1)
                            sqlBuilder.Append(",");
                        else if (mod == 0 || t == entities.Count - 1)
                        {
                            sqlBuilder.Append(";");

                            var sql = sqlBuilder.ToString();
                            await Execute(sql, uow: uow);
                            _logger.Write("BulkAdd", sql);
                        }
                    }

                    #endregion
                }

                if (!hasTran)
                    uow.SaveChanges();

                return true;
            }
            catch
            {
                if (!hasTran)
                    uow.Rollback();

                throw;
            }
        }
    }
}
