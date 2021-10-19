using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mkh.Data.Abstractions.Queryable.Grouping;

/// <summary>
/// 分组查询
/// </summary>
public interface IGroupingQueryable
{
    #region ==List==

    /// <summary>
    /// 查询列表，返回Dynamic类型
    /// </summary>
    /// <returns></returns>
    Task<IList<dynamic>> ToListDynamic();

    /// <summary>
    /// 查询列表，返回指定类型
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    Task<IList<TResult>> ToList<TResult>();

    /// <summary>
    /// 获取查询列表SQL
    /// </summary>
    /// <returns></returns>
    string ToListSql();

    /// <summary>
    /// 获取查询列表SQL，并返回参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    string ToListSql(out IQueryParameters parameters);

    /// <summary>
    /// 获取查询列表SQL，并设置参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    string ToListSql(IQueryParameters parameters);

    /// <summary>
    /// 获取查询列表SQL，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    string ToListSqlNotUseParameters();

    #endregion

    #region ==Reader==

    /// <summary>
    /// 查询列表，返回IDataReader
    /// </summary>
    /// <returns></returns>
    Task<IDataReader> ToReader();

    #endregion

    #region ==First==

    /// <summary>
    /// 查询第一条数据，返回Dynamic类型
    /// </summary>
    /// <returns></returns>
    Task<dynamic> ToFirstDynamic();

    /// <summary>
    /// 查询第一条数据，返回指定类型
    /// </summary>
    /// <returns></returns>
    Task<TResult> ToFirst<TResult>();

    /// <summary>
    /// 查询第一条数据的SQL
    /// </summary>
    /// <returns></returns>
    string ToFirstSql();

    /// <summary>
    /// 查询第一条数据的SQL，并返回参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    string ToFirstSql(out IQueryParameters parameters);

    /// <summary>
    /// 查询第一条数据的SQL，并设置参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    string ToFirstSql(IQueryParameters parameters);

    /// <summary>
    /// 查询第一条数据的SQL，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    string ToFirstSqlNotUseParameters();

    #endregion
}

/// <summary>
/// 分组查询对象
/// </summary>
public interface IGrouping<out TKey>
{
    TKey Key { get; }

    long Count();
}