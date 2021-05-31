using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Core.SqlBuilder;

namespace Mkh.Data.Core.Descriptors
{
    /// <summary>
    /// 实体信息描述符
    /// </summary>
    internal class EntityDescriptor : IEntityDescriptor
    {
        #region ==属性==

        /// <summary>
        /// 数据库上下文
        /// </summary>
        public IDbContext DbContext { get; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// 自动创建表
        /// </summary>
        public bool AutoCreate { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// 主键列
        /// </summary>
        public IPrimaryKeyDescriptor PrimaryKey { get; private set; }

        /// <summary>
        /// 列集合
        /// </summary>
        public IList<IColumnDescriptor> Columns { get; }

        /// <summary>
        /// SQL语句描述符
        /// </summary>
        public IEntitySqlDescriptor SqlDescriptor { get; }

        /// <summary>
        /// 是否使用实体基类
        /// </summary>
        public bool IsEntityBase { get; set; }

        /// <summary>
        /// 是否使用租户
        /// </summary>
        public bool IsTenant { get; }

        /// <summary>
        /// 是否使用软删除
        /// </summary>
        public bool IsSoftDelete { get; set; }

        #endregion

        #region ==构造函数==

        public EntityDescriptor(IDbContext dbContext, Type entityType)
        {
            Columns = new List<IColumnDescriptor>();
            DbContext = dbContext;
            EntityType = entityType;
            IsEntityBase = EntityType.IsSubclassOfGeneric(typeof(EntityBase<>));
            IsTenant = typeof(ITenant).IsAssignableFrom(EntityType);
            IsSoftDelete = typeof(ISoftDelete).IsImplementType(EntityType);

            SetTableName();

            SetColumns();

            SqlDescriptor = new CrudSqlBuilder(this).Build();
        }

        #endregion

        #region ==私有方法==

        /// <summary>
        /// 设置表名
        /// </summary>
        private void SetTableName()
        {
            var tableArr = EntityType.GetCustomAttribute<TableAttribute>(false);
            if (tableArr != null && tableArr.Name.NotNull())
            {
                TableName = tableArr.Name;
            }
            else
            {
                //去掉Entity后缀
                TableName = EntityType.Name.Substring(0, EntityType.Name.Length - 6);
            }

            var setPrefix = tableArr != null ? tableArr.SetPrefix : true;
            //设置前缀
            if (setPrefix && DbContext.Options.TablePrefix.NotNull())
            {
                TableName = DbContext.Options.TablePrefix + TableName;
            }

            //设置自动创建表
            AutoCreate = tableArr != null ? tableArr.AutoCreate : true;

            //表名称小写
            if (DbContext.Adapter.SqlLowerCase)
            {
                TableName = TableName.ToLower();
            }
        }

        /// <summary>
        /// 设置属性列表
        /// </summary>
        private void SetColumns()
        {
            //加载属性列表
            var properties = new List<PropertyInfo>();
            foreach (var p in EntityType.GetProperties())
            {
                var type = p.PropertyType;
                if (type == typeof(TimeSpan) || (!type.IsGenericType || type.IsNullable()) && (type.IsGuid() || type.IsNullable() || Type.GetTypeCode(type) != TypeCode.Object)
                    && Attribute.GetCustomAttributes(p).All(attr => attr.GetType() != typeof(NotMappingColumnAttribute)))
                {
                    properties.Add(p);
                }
            }

            foreach (var p in properties)
            {
                var column = new ColumnDescriptor(p, DbContext.Adapter);

                if (column.IsPrimaryKey)
                {
                    PrimaryKey = new PrimaryKeyDescriptor(p);
                    Columns.Insert(0, column);
                }
                else
                {
                    Columns.Add(column);
                }
            }

            //如果主键为null，则需要指定为没有主键
            if (PrimaryKey == null)
            {
                PrimaryKey = new PrimaryKeyDescriptor();
            }
        }

        #endregion
    }
}
