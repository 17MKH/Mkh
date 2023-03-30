using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Pagination;

namespace Mkh.Data.Abstractions.Queryable;

/// <summary>
/// 查询构造器
/// </summary>
public interface IQueryable
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

    #region ==Pagination==

    /// <summary>
    /// 分页查询，返回Dynamic类型
    /// </summary>
    /// <returns></returns>
    Task<IList<dynamic>> ToPaginationDynamic();

    /// <summary>
    /// 分页查询，返回Dynamic类型
    /// </summary>
    /// <param name="paging">分页对象</param>
    /// <returns></returns>
    Task<IList<dynamic>> ToPaginationDynamic(Paging paging);

    /// <summary>
    /// 分页查询，返回指定类型
    /// </summary>
    /// <returns></returns>
    Task<IList<TResult>> ToPagination<TResult>();

    /// <summary>
    /// 分页查询，返回指定类型
    /// </summary>
    /// <param name="paging">分页对象</param>
    /// <returns></returns>
    Task<IList<TResult>> ToPagination<TResult>(Paging paging);

    /// <summary>
    /// 获取分页查询列表SQL
    /// </summary>
    /// <returns></returns>
    string ToPaginationSql(Paging paging);

    /// <summary>
    /// 获取分页查询列表SQL，并返回参数
    /// </summary>
    /// <param name="paging">分页信息</param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    string ToPaginationSql(Paging paging, out IQueryParameters parameters);

    /// <summary>
    /// 获取分页查询列表SQL，并设置参数
    /// </summary>
    /// <param name="paging">分页信息</param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    string ToPaginationSql(Paging paging, IQueryParameters parameters);

    /// <summary>
    /// 获取分页查询列表SQL，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    string ToPaginationSqlNotUseParameters(Paging paging);

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

    #region ==Count==

    /// <summary>
    /// 查询数量
    /// </summary>
    /// <returns></returns>
    Task<long> ToCount();

    /// <summary>
    /// 查询数量的SQL
    /// </summary>
    /// <returns></returns>
    string ToCountSql();

    /// <summary>
    /// 查询数量的SQL，并返回参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    string ToCountSql(out IQueryParameters parameters);

    /// <summary>
    /// 查询数量的SQL，并设置参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    string ToCountSql(IQueryParameters parameters);

    /// <summary>
    /// 查询数量的SQL，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    string ToCountSqlNotUseParameters();

    #endregion

    #region ==Exists==

    /// <summary>
    /// 判断是否存在
    /// </summary>
    /// <returns></returns>
    Task<bool> ToExists();

    /// <summary>
    /// 判断是否存在的SQL
    /// </summary>
    /// <returns></returns>
    string ToExistsSql();

    /// <summary>
    /// 判断是否存在的SQL，并返回参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    string ToExistsSql(out IQueryParameters parameters);

    /// <summary>
    /// 判断是否存在的SQL，并设置参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    string ToExistsSql(IQueryParameters parameters);

    /// <summary>
    /// 判断是否存在的SQL，并且不使用参数化
    /// </summary>
    /// <returns></returns>
    string ToExistsSqlNotUseParameters();

    #endregion
}