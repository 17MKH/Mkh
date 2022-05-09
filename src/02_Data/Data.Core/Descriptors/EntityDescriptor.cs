using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Sharding;
using Mkh.Data.Core.SqlBuilder;

namespace Mkh.Data.Core.Descriptors;

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
    /// 实体名称
    /// </summary>
    public string Name { get; }

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

    /// <summary>
    /// 禁用新增事件
    /// </summary>
    public bool EnableAddEvent { get; set; }

    /// <summary>
    /// 禁用更新事件
    /// </summary>
    public bool EnableUpdateEvent { get; set; }

    /// <summary>
    /// 禁用删除事件
    /// </summary>
    public bool EnableDeleteEvent { get; set; }

    /// <summary>
    /// 禁用软删除事件
    /// </summary>
    public bool EnableSoftDeleteEvent { get; set; }

    /// <summary>
    /// 是否分表
    /// </summary>
    public bool IsSharding { get; set; }

    /// <summary>
    /// 分表策略
    /// </summary>
    public ShardingPolicy ShardingPolicy { get; set; }

    /// <summary>
    /// 分表提供器
    /// </summary>
    public IShardingPolicyProvider ShardingPolicyProvider { get; set; }

    /// <summary>
    /// 自定义分表策略提供器类型
    /// </summary>
    public Type CustomShardingPolicyProviderType { get; set; }

    #endregion

    #region ==构造函数==

    public EntityDescriptor(IDbContext dbContext, Type entityType)
    {
        Columns = new List<IColumnDescriptor>();
        DbContext = dbContext;
        Name = entityType.Name.Substring(0, entityType.Name.Length - 6);
        EntityType = entityType;
        IsEntityBase = EntityType.IsSubclassOfGeneric(typeof(EntityBase<>));
        IsTenant = typeof(ITenant).IsAssignableFrom(EntityType);
        IsSoftDelete = typeof(ISoftDelete).IsImplementType(EntityType);

        SetTableName();

        SetColumns();

        SetEvents();

        SetSharding();

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
            TableName = Name;

            //给表名称添加分隔符
            if (DbContext.Options.TableNameSeparator.NotNull())
            {
                var matchs = Regex.Matches(TableName, "[A-Z][^A-Z]+");
                TableName = string.Join(DbContext.Options.TableNameSeparator, matchs);
            }
        }

        var setPrefix = tableArr?.SetPrefix ?? true;
        //设置前缀
        if (setPrefix && DbContext.Options.TableNamePrefix.NotNull())
        {
            TableName = DbContext.Options.TableNamePrefix + TableName;
        }

        //设置自动创建表
        AutoCreate = tableArr?.AutoCreate ?? true;

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

    /// <summary>
    /// 设置事件
    /// </summary>
    private void SetEvents()
    {
        if (EntityType.GetCustomAttribute<EnableEntityAllEvent>(false) != null)
        {
            EnableAddEvent = true;
            EnableUpdateEvent = true;
            EnableDeleteEvent = true;
            EnableSoftDeleteEvent = true;

            return;
        }

        EnableAddEvent = EntityType.GetCustomAttribute<EnableEntityAddEvent>(false) != null;
        EnableUpdateEvent = EntityType.GetCustomAttribute<EnableEntityUpdateEvent>(false) != null;
        EnableDeleteEvent = EntityType.GetCustomAttribute<EnableEntityDeleteEvent>(false) != null;
        EnableSoftDeleteEvent = EntityType.GetCustomAttribute<EnableEntitySoftDeleteEvent>(false) != null;
    }

    /// <summary>
    /// 设置分表信息
    /// </summary>
    private void SetSharding()
    {
        var sharding = EntityType.GetCustomAttribute<ShardingAttribute>(false);
        if (sharding != null)
        {
            IsSharding = true;
            ShardingPolicy = sharding.Policy;
            CustomShardingPolicyProviderType = sharding.CustomProviderType;
        }
    }

    #endregion
}