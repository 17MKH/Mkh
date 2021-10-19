using System;
using System.Linq.Expressions;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Data.Abstractions.Queryable.Grouping;

/// <summary>
/// 分组查询对象
/// </summary>
public interface IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7> : IGroupingQueryable
    where TEntity : IEntity, new()
    where TEntity2 : IEntity, new()
    where TEntity3 : IEntity, new()
    where TEntity4 : IEntity, new()
    where TEntity5 : IEntity, new()
    where TEntity6 : IEntity, new()
    where TEntity7 : IEntity, new()
{
    /// <summary>
    /// 聚合过滤
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7> Having(Expression<Func<IGrouping<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7>, bool>> expression);

    /// <summary>
    /// 聚合过滤
    /// </summary>
    /// <param name="havingSql">SQL语句</param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7> Having(string havingSql);

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="field">排序字段名称</param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7> OrderBy(string field);

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="field">排序字段名称</param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7> OrderByDescending(string field);

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7> OrderBy<TResult>(Expression<Func<IGrouping<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7>, TResult>> expression);

    /// <summary>
    /// 倒序排序
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7> OrderByDescending<TResult>(Expression<Func<IGrouping<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7>, TResult>> expression);

    /// <summary>
    /// 查询列
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7> Select<TResult>(Expression<Func<IGrouping<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7>, TResult>> expression);
}

public interface IGrouping<out TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7> : IGrouping<TKey>
    where TEntity : IEntity, new()
    where TEntity2 : IEntity, new()
    where TEntity3 : IEntity, new()
    where TEntity4 : IEntity, new()
    where TEntity5 : IEntity, new()
    where TEntity6 : IEntity, new()
    where TEntity7 : IEntity, new()
{
    TResult Max<TResult>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7>, TResult>> where);

    TResult Min<TResult>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7>, TResult>> where);

    TResult Sum<TResult>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7>, TResult>> where);

    TResult Avg<TResult>(Expression<Func<IQueryableJoins<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, TEntity7>, TResult>> where);
}