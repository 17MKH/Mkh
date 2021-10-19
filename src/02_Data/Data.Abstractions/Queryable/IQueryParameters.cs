using System.Collections.Generic;
using Dapper;

namespace Mkh.Data.Abstractions.Queryable;

/// <summary>
/// 参数集
/// </summary>
public interface IQueryParameters
{
    /// <summary>
    /// 参数数量
    /// </summary>
    int Count { get; }

    /// <summary>
    /// 添加参数，返回参数名称
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    string Add(object value);

    /// <summary>
    /// 转换为Dapper使用的DynamicParameters
    /// </summary>
    /// <returns></returns>
    DynamicParameters ToDynamicParameters();

    /// <summary>
    /// 清空参数
    /// </summary>
    void Clear();

    /// <summary>
    /// 索引
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    KeyValuePair<string, object> this[int index] { get; }
}