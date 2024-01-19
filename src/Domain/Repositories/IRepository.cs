using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Mkh.Domain.Entities;
using Mkh.Domain.Exceptions;
using Mkh.Domain.Repositories.Query;

namespace Mkh.Domain.Repositories;

/// <summary>
/// 仓储接口
/// </summary>
public interface IRepository
{

}

/// <summary>
/// 泛型仓储接口
/// </summary>
public interface IRepository<TEntity> : IRepository where TEntity : IEntity
{
    /// <summary>
    /// 插入一个实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 插入多个实体
    /// </summary>
    /// <param name="entities">实体集合</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<bool> InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除一个实体
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除多个实体
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新多个实体
    /// </summary>
    /// <param name="entities">多个实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据主键获取单个实体，如果指定的主键查询不到实体，会抛出异常 <see cref="EntityNotFoundException"/>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <exception cref="EntityNotFoundException"></exception>
    /// <returns></returns>
    Task<TEntity> GetAsync(dynamic id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取总数量
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 判断是否存在
    /// </summary>
    /// <param name="expression">查询条件</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取一个查询构造器 <see cref="IQueryBuilder{TEntity}"/>
    /// </summary>
    /// <returns></returns>
    IQueryBuilder<TEntity> Find();

    /// <summary>
    /// 获取一个查询构造器 <see cref="IQueryBuilder{TEntity}"/>
    /// </summary>
    /// <returns></returns>
    IQueryBuilder<TEntity> Find(Expression<Func<TEntity, bool>> expression);
}