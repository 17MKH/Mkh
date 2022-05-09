using System;
using System.Collections.Generic;
using Mkh.Data.Abstractions.Sharding;

namespace Mkh.Data.Abstractions.Descriptors;

/// <summary>
/// 实体信息描述符
/// </summary>
public interface IEntityDescriptor
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    IDbContext DbContext { get; }

    /// <summary>
    /// 实体名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 表名称
    /// </summary>
    string TableName { get; }

    /// <summary>
    /// 自动创建表
    /// </summary>
    bool AutoCreate { get; set; }

    /// <summary>
    /// 实体类型
    /// </summary>
    Type EntityType { get; }

    /// <summary>
    /// 主键列
    /// </summary>
    IPrimaryKeyDescriptor PrimaryKey { get; }

    /// <summary>
    /// 列集合
    /// </summary>
    IList<IColumnDescriptor> Columns { get; }

    /// <summary>
    /// SQL语句描述符
    /// </summary>
    IEntitySqlDescriptor SqlDescriptor { get; }

    /// <summary>
    /// 是否继承实体基类
    /// </summary>
    bool IsEntityBase { get; }

    /// <summary>
    /// 是否使用租户
    /// </summary>
    bool IsTenant { get; }

    /// <summary>
    /// 是否使用软删除
    /// </summary>
    bool IsSoftDelete { get; }

    /// <summary>
    /// 启用新增事件
    /// </summary>
    bool EnableAddEvent { get; }

    /// <summary>
    /// 启用更新事件
    /// </summary>
    bool EnableUpdateEvent { get; }

    /// <summary>
    /// 启用删除事件
    /// </summary>
    bool EnableDeleteEvent { get; }

    /// <summary>
    /// 启用软删除事件
    /// </summary>
    bool EnableSoftDeleteEvent { get; }

    /// <summary>
    /// 是否分表
    /// </summary>
    bool IsSharding { get; }

    /// <summary>
    /// 分表策略
    /// </summary>
    ShardingPolicy ShardingPolicy { get; }

    /// <summary>
    /// 分表策略提供器
    /// </summary>
    IShardingPolicyProvider ShardingPolicyProvider { get; }

    /// <summary>
    /// 自定义分表策略提供器类型
    /// </summary>
    Type CustomShardingPolicyProviderType { get; }
}