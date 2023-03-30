using System.Collections.Generic;
using System.Data;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Logger;
using Mkh.Data.Abstractions.Options;
using Mkh.Data.Abstractions.Schema;

namespace Mkh.Data.Abstractions;

/// <summary>
/// 数据库上下文
/// </summary>
public interface IDbContext
{
    #region ==属性==

    /// <summary>
    /// 数据库配置项
    /// </summary>
    DbOptions Options { get; }

    /// <summary>
    /// 日志记录器
    /// </summary>
    DbLogger Logger { get; }

    /// <summary>
    /// 数据库适配器
    /// </summary>
    IDbAdapter Adapter { get; }

    /// <summary>
    /// 数据库架构提供器
    /// </summary>
    ISchemaProvider SchemaProvider { get; }

    /// <summary>
    /// 代码生成提供器
    /// </summary>
    ICodeFirstProvider CodeFirstProvider { get; }

    /// <summary>
    /// 账户信息解析器
    /// </summary>
    IOperatorResolver AccountResolver { get; }

    /// <summary>
    /// 实体描述符列表
    /// </summary>
    IList<IEntityDescriptor> EntityDescriptors { get; }

    /// <summary>
    /// 仓储描述符列表
    /// </summary>
    IList<IRepositoryDescriptor> RepositoryDescriptors { get; }

    #endregion

    #region ==方法==

    /// <summary>
    /// 创建新的连接
    /// </summary>
    IDbConnection NewConnection();

    /// <summary>
    /// 使用指定字符串创建连接
    /// </summary>
    /// <param name="connectionString">连接字符串</param>
    /// <returns></returns>
    IDbConnection NewConnection(string connectionString);

    /// <summary>
    /// 创建工作单元
    /// </summary>
    /// <param name="isolationLevel">指定锁级别</param>
    /// <returns></returns>
    IUnitOfWork NewUnitOfWork(IsolationLevel? isolationLevel = null);

    #endregion
}