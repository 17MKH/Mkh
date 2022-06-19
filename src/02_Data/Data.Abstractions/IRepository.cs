using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Queryable;

namespace Mkh.Data.Abstractions;

/// <summary>
/// 仓储接口
/// </summary>
public interface IRepository
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    IDbContext DbContext { get; }

    /// <summary>
    /// 关联工作单元
    /// </summary>
    IUnitOfWork Uow { get; }

    /// <summary>
    /// 实体描述符
    /// </summary>
    IEntityDescriptor EntityDescriptor { get; }

    /// <summary>
    /// 绑定工作单元
    /// </summary>
    /// <param name="uow"></param>
    void BindingUow(IUnitOfWork uow);

    /// <summary>
    /// 执行一个命令并返回受影响的行数
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="param">参数</param>
    /// <param name="uow">工作单元</param>
    /// <param name="commandType">命令类型</param>
    /// <returns></returns>
    Task<int> Execute(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null);

    /// <summary>
    /// 执行一个命令并返回单个值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql">sql语句</param>
    /// <param name="param">参数</param>
    /// <param name="uow">工作单元</param>
    /// <param name="commandType">命令类型</param>
    /// <returns></returns>
    Task<T> ExecuteScalar<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null);

    /// <summary>
    /// 执行SQL语句并返回IDataReader
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="param">参数</param>
    /// <param name="uow">工作单元</param>
    /// <param name="commandType">命令类型</param>
    /// <returns></returns>
    Task<IDataReader> ExecuteReader(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null);

    /// <summary>
    /// 查询第一条数据，不存在返回默认值
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="param">参数</param>
    /// <param name="uow">工作单元</param>
    /// <param name="commandType">命令类型</param>
    /// <returns></returns>
    Task<dynamic> QueryFirstOrDefault(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null);

    /// <summary>
    /// 查询第一条数据并返回指定类型，不存在返回默认值
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="param">参数</param>
    /// <param name="uow">工作单元</param>
    /// <param name="commandType">命令类型</param>
    /// <returns></returns>
    Task<T> QueryFirstOrDefault<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null);

    /// <summary>
    /// 查询单条记录，不存在返回默认值，如果存在多条记录则抛出异常
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="param">参数</param>
    /// <param name="uow">工作单元</param>
    /// <param name="commandType">命令类型</param>
    /// <returns></returns>
    Task<dynamic> QuerySingleOrDefault(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null);

    /// <summary>
    /// 查询单条记录并返回指定类型，不存在返回默认值，如果存在多条记录则抛出异常
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="param">参数</param>
    /// <param name="uow">工作单元</param>
    /// <param name="commandType">命令类型</param>
    /// <returns></returns>
    Task<T> QuerySingleOrDefault<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null);

    /// <summary>
    /// 查询多条结果
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="param">参数</param>
    /// <param name="uow">工作单元</param>
    /// <param name="commandType">命令类型</param>
    /// <returns></returns>
    Task<SqlMapper.GridReader> QueryMultiple(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null);

    /// <summary>
    /// 查询结果集，返回动态类型
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="param">参数</param>
    /// <param name="uow">工作单元</param>
    /// <param name="commandType">命令类型</param>
    /// <returns></returns>
    Task<IEnumerable<dynamic>> Query(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null);

    /// <summary>
    /// 查询数据列表并返回指定类型
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <param name="param">参数</param>
    /// <param name="uow">工作单元</param>
    /// <param name="commandType">命令类型</param>
    /// <returns></returns>
    Task<IEnumerable<T>> Query<T>(string sql, object param = null, IUnitOfWork uow = null, CommandType? commandType = null);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> Delete(dynamic id, IUnitOfWork uow = null);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="tableName">表名</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> Delete(dynamic id, string tableName, IUnitOfWork uow = null);

    /// <summary>
    /// 软删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> SoftDelete(dynamic id, IUnitOfWork uow = null);

    /// <summary>
    /// 软删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="tableName">表名</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> SoftDelete(dynamic id, string tableName, IUnitOfWork uow = null);

    /// <summary>
    /// 根据主键判断是否存在
    /// </summary>
    /// <param name="id"></param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> Exists(dynamic id, IUnitOfWork uow = null);

    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="tableName">表名</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> Exists(dynamic id, string tableName, IUnitOfWork uow = null);

    /// <summary>
    /// 获取当前时间对应的分表表名
    /// </summary>
    /// <returns></returns>
    string GetShardingTableName();

    /// <summary>
    /// 获取指定时间分表表名
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    string GetShardingTableName(DateTime date);
}

/// <summary>
/// 泛型仓储接口
/// </summary>
public interface IRepository<TEntity> : IRepository where TEntity : IEntity, new()
{
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> Add(TEntity entity, IUnitOfWork uow = null);

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="tableName">表名</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> Add(TEntity entity, string tableName, IUnitOfWork uow = null);

    /// <summary>
    /// 批量添加
    /// </summary>
    /// <param name="entities">实体集合</param>
    /// <param name="flushSize">单次刷新数量</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> BulkAdd(IList<TEntity> entities, int flushSize = 0, IUnitOfWork uow = null);

    /// <summary>
    /// 批量添加
    /// </summary>
    /// <param name="entities">实体集合</param>
    /// <param name="tableName">表名</param>
    /// <param name="flushSize">单次刷新数量</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> BulkAdd(IList<TEntity> entities, string tableName, int flushSize = 0, IUnitOfWork uow = null);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> Update(TEntity entity, IUnitOfWork uow = null);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="tableName">表名</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<bool> Update(TEntity entity, string tableName, IUnitOfWork uow = null);

    /// <summary>
    /// 根据主键获取单个实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<TEntity> Get(dynamic id, IUnitOfWork uow = null);

    /// <summary>
    /// 根据主键获取单个实体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tableName">表名</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<TEntity> Get(dynamic id, string tableName, IUnitOfWork uow = null);

    /// <summary>
    /// 根据条件获取单个实体
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<TEntity> Get(Expression<Func<TEntity, bool>> expression, IUnitOfWork uow = null);

    /// <summary>
    /// 根据条件获取单个实体
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="tableName">表名</param>
    /// <param name="uow">工作单元</param>
    /// <returns></returns>
    Task<TEntity> Get(Expression<Func<TEntity, bool>> expression, string tableName, IUnitOfWork uow = null);

    /// <summary>
    /// 查询
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntity> Find();

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    IQueryable<TEntity> Find(string tableName);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="noLock">SqlServer的WITH (NOLOCK)特性，为true时添加，默认：true</param>
    /// <returns></returns>
    IQueryable<TEntity> Find(string tableName, bool noLock);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="noLock">SqlServer的WITH (NOLOCK)特性，为true时添加，默认：true</param>
    /// <returns></returns>
    IQueryable<TEntity> Find(bool noLock);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="expression">过滤条件</param>
    /// <returns></returns>
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="expression">过滤条件</param>
    /// <param name="tableName">表名</param>
    /// <returns></returns>
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, string tableName);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="expression">过滤条件</param>
    /// <param name="noLock">SqlServer的WITH (NOLOCK)特性，为true时添加，默认：true</param>
    /// <returns></returns>
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, bool noLock);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="expression">过滤条件</param>
    /// <param name="tableName">表名</param>
    /// <param name="noLock">SqlServer的WITH (NOLOCK)特性，为true时添加，默认：true</param>
    /// <returns></returns>
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, string tableName, bool noLock);
}