using System;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Data.Core.Repository
{
    /// <summary>
    /// 新增
    /// </summary>
    public abstract partial class RepositoryAbstract<TEntity>
    {
        public Task<bool> Add(dynamic entity, IUnitOfWork uow = null)
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

            return await Execute(sql, entity) > 0;
        }

        /// <summary>
        /// 设置创建信息
        /// </summary>
        private void SetCreateInfo(IEntity entity)
        {
            //设置实体的添加人编号、添加人姓名、添加时间
            var descriptor = EntityDescriptor;
            if (descriptor.IsEntityBase)
            {
                foreach (var column in descriptor.Columns)
                {
                    var colName = column.PropertyInfo.Name;
                    if (colName.Equals("CreatedBy"))
                    {
                        var createdBy = column.PropertyInfo.GetValue(entity);
                        if (createdBy == null || (Guid)createdBy == Guid.Empty)
                        {
                            column.PropertyInfo.SetValue(entity, DbContext.AccountResolver.AccountId);
                        }
                        continue;
                    }
                    if (colName.Equals("Creator"))
                    {
                        var creator = column.PropertyInfo.GetValue(entity);
                        if (creator == null)
                        {
                            column.PropertyInfo.SetValue(entity, DbContext.AccountResolver.AccountName);
                        }
                        continue;
                    }
                    if (colName.Equals("CreatedTime"))
                    {
                        var createdTime = column.PropertyInfo.GetValue(entity);
                        if (createdTime == null)
                        {
                            column.PropertyInfo.SetValue(entity, DateTime.Now);
                        }
                    }
                }
            }
        }
    }
}

