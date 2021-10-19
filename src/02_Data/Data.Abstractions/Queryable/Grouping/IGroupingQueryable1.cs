using System;
using System.Linq.Expressions;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Data.Abstractions.Queryable.Grouping;

/// <summary>
/// 单表分组查询对象
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IGroupingQueryable<TKey, TEntity> : IGroupingQueryable where TEntity : IEntity, new()
{
    /// <summary>
    /// 聚合过滤
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity> Having(Expression<Func<IGrouping<TKey, TEntity>, bool>> expression);

    /// <summary>
    /// 聚合过滤
    /// </summary>
    /// <param name="havingSql">SQL语句</param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity> Having(string havingSql);

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="field">排序字段名称</param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity> OrderBy(string field);

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="field">排序字段名称</param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity> OrderByDescending(string field);

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity> OrderBy<TResult>(Expression<Func<IGrouping<TKey, TEntity>, TResult>> expression);

    /// <summary>
    /// 倒序排序
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity> OrderByDescending<TResult>(Expression<Func<IGrouping<TKey, TEntity>, TResult>> expression);

    /// <summary>
    /// 查询列
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity> Select<TResult>(Expression<Func<IGrouping<TKey, TEntity>, TResult>> expression);
}

public interface IGrouping<out TKey, TEntity> : IGrouping<TKey> where TEntity : IEntity, new()
{
    TResult Max<TResult>(Expression<Func<TEntity, TResult>> where);

    TResult Min<TResult>(Expression<Func<TEntity, TResult>> where);

    TResult Sum<TResult>(Expression<Func<TEntity, TResult>> where);

    TResult Avg<TResult>(Expression<Func<TEntity, TResult>> where);
}