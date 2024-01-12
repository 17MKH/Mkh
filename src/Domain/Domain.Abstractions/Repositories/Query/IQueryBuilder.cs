using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Mkh.Domain.Abstractions.Entities;

namespace Mkh.Domain.Abstractions.Repositories.Query;

public interface IQueryBuilder<TEntity> where TEntity : IEntity, new()
{
    #region ==Sort==

    /// <summary>
    /// 升序
    /// </summary>
    /// <param name="field">排序字段名称</param>
    /// <returns></returns>
    IQueryBuilder<TEntity> OrderBy(string field);

    /// <summary>
    /// 降序
    /// </summary>
    /// <param name="field">排序字段名称</param>
    /// <returns></returns>
    IQueryBuilder<TEntity> OrderByDescending(string field);

    /// <summary>
    /// 升序
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> expression);

    /// <summary>
    /// 降序
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> expression);

    #endregion

    #region ==Where==

    /// <summary>
    /// 过滤
    /// </summary>
    /// <param name="expression">过滤条件</param>
    /// <returns></returns>
    IQueryBuilder<TEntity> Where(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// 条件为true时添加过滤
    /// </summary>
    /// <param name="condition">添加条件</param>
    /// <param name="expression">条件</param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereIf(bool condition, Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// 条件为true时添加SQL语句条件
    /// </summary>
    /// <param name="condition">添加条件</param>
    /// <param name="whereSql">查询条件</param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereIf(bool condition, string whereSql);

    /// <summary>
    /// 根据条件添加过滤
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="ifExpression"></param>
    /// <param name="elseExpression"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereIfElse(bool condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression);

    /// <summary>
    /// 根据条件添加SQL语句条件
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="ifWhereSql"></param>
    /// <param name="elseWhereSql"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereIfElse(bool condition, string ifWhereSql, string elseWhereSql);

    /// <summary>
    /// 字符串不为Null以及空字符串的时候添加过滤
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotNull(string condition, Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// 字符串不为Null以及空字符串的时候添加ifExpression，反之添加elseExpression
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="ifExpression"></param>
    /// <param name="elseExpression"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotNull(string condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression);

    /// <summary>
    /// 字符串不为Null以及空字符串的时候添加SQL语句条件
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="whereSql"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotNull(string condition, string whereSql);

    /// <summary>
    /// 字符串不为Null以及空字符串的时候添加ifWhereSql，反之添加elseWhereSql
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="ifWhereSql"></param>
    /// <param name="elseWhereSql"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotNull(string condition, string ifWhereSql, string elseWhereSql);

    /// <summary>
    /// 对象不为Null以及空字符串的时候添加SQL语句条件
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotNull(object condition, Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// 对象不为Null的时候添加ifExpression，反之添加elseExpression
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="ifExpression"></param>
    /// <param name="elseExpression"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotNull(object condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression);

    /// <summary>
    /// 对象不为Null以及空字符串的时候添加SQL语句条件
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="whereSql"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotNull(object condition, string whereSql);

    /// <summary>
    /// 对象不为Null的时候添加ifWhereSql，反之添加elseWhereSql
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="ifWhereSql"></param>
    /// <param name="elseWhereSql"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotNull(object condition, string ifWhereSql, string elseWhereSql);

    /// <summary>
    /// GUID不为空的时候添加过滤条件
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotEmpty(Guid condition, Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// GUID不为空的时候添加过滤SQL语句条件
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="whereSql"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotEmpty(Guid condition, string whereSql);

    /// <summary>
    /// GUID不为空的时候添加ifExpression，反之添加elseExpression
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="ifExpression"></param>
    /// <param name="elseExpression"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotEmpty(Guid condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression);

    /// <summary>
    /// GUID不为空的时候添加ifExpression，反之添加elseExpression
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="ifWhereSql"></param>
    /// <param name="elseWhereSql"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> WhereNotEmpty(Guid condition, string ifWhereSql, string elseWhereSql);

    #endregion

    #region ==Select==

    /// <summary>
    /// 查询返回指定列
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression">返回的列</param>
    /// <returns></returns>
    IQueryBuilder<TEntity> Select<TResult>(Expression<Func<TEntity, TResult>> expression);

    /// <summary>
    /// 查询返回指定列
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="sql">SELECT后面的SQL语句，一般用于需要自定义的情况</param>
    /// <returns></returns>
    IQueryBuilder<TEntity> Select<TResult>(string sql);

    /// <summary>
    /// 查询排除指定列
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    IQueryBuilder<TEntity> SelectExclude<TResult>(Expression<Func<TEntity, TResult>> expression);

    #endregion

    #region ==Limit==

    /// <summary>
    /// 限制
    /// </summary>
    /// <param name="skip">跳过前几条数据</param>
    /// <param name="take">取前几条数据</param>
    /// <returns></returns>
    IQueryBuilder<TEntity> Limit(int skip, int take);

    #endregion

    #region ==NotFilterSoftDeleted==

    /// <summary>
    /// 不过滤软删除数据
    /// </summary>
    /// <returns></returns>
    IQueryBuilder<TEntity> NotFilterSoftDeleted();

    #endregion

    #region ==NotFilterTenant==

    /// <summary>
    /// 不过滤租户
    /// </summary>
    /// <returns></returns>
    IQueryBuilder<TEntity> NotFilterTenant();

    #endregion

    #region ==Query==

    /// <summary>
    /// 查询列表，返回指定类型
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    IList<TResult> ToList<TResult>(CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询列表，返回指定类型
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<IList<TResult>> ToListAsync<TResult>(CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询第一条数据，返回指定类型
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    TResult FirstOrDefault<TResult>(CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询第一条数据，返回指定类型
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<TResult> FirstOrDefaultAsync<TResult>(CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询数量
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    long Count(CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询数量
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<long> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 判断是否存在
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    bool Exists(CancellationToken cancellationToken = default);

    /// <summary>
    /// 判断是否存在
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<bool> ExistsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询列表，返回实体类型
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    IList<TEntity> ToList(CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询列表，返回实体类型
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<IList<TEntity>> ToListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询第一条数据，如果没有数据则返回实体类型的默认值
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

    #endregion

    #region ==Function==

    /// <summary>
    /// 获取最大值
    /// </summary>
    /// <param name="expression">指定要获取最大值的属性</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取最小值
    /// </summary>
    /// <param name="expression">指定要获取最小值的属性</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// 求和
    /// </summary>
    /// <param name="expression">指定要求和的属性</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<TResult> SumAsync<TResult>(Expression<Func<TEntity, TResult>> expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// 求平均值
    /// </summary>
    /// <param name="expression">指定要求平均值的属性</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<TResult> AvgAsync<TResult>(Expression<Func<TEntity, TResult>> expression, CancellationToken cancellationToken = default);

    #endregion

    #region ==Copy==

    /// <summary>
    /// 克隆一个新的实例
    /// </summary>
    /// <returns></returns>
    IQueryBuilder<TEntity> Clone();

    #endregion

    #region ==Delete==

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>数据库影响的行数</returns>
    Task<bool> DeleteAsync(CancellationToken cancellationToken = default);

    #endregion
}