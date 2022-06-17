using System;
using System.Collections.Generic;

namespace Mkh.Data.Abstractions.Query;

/// <summary>
/// 分页查询结果模型
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagingQueryResultModel<T> : IResultModel<PagingQueryResultBody<T>>
{
    /// <summary>
    /// 处理是否成功
    /// </summary>
    public bool Successful { get; private set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string Msg { get; private set; }

    /// <summary>
    /// 业务码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public long Timestamp { get; private set; }

    /// <summary>
    /// 返回数据
    /// </summary>
    public PagingQueryResultBody<T> Data { get; private set; }

    public PagingQueryResultModel()
    {
        Timestamp = DateTime.Now.ToTimestamp();
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="msg">说明</param>
    public PagingQueryResultModel<T> Success(PagingQueryResultBody<T> data = default, string msg = "success")
    {
        Successful = true;
        Data = data;
        Msg = msg;

        return this;
    }

    /// <summary>
    /// 失败
    /// </summary>
    /// <param name="msg">说明</param>
    public PagingQueryResultModel<T> Failed(string msg = "failed")
    {
        Successful = false;
        Msg = msg;
        return this;
    }

}

/// <summary>
/// 分页查询结果内容
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagingQueryResultBody<T>
{
    /// <summary>
    /// 总数
    /// </summary>
    public long Total { get; set; }

    /// <summary>
    /// 数据集
    /// </summary>
    public IList<T> Rows { get; set; }

    /// <summary>
    /// 扩展数据
    /// </summary>
    public object Data { get; set; }

    public PagingQueryResultBody(IList<T> rows, long total)
    {
        Rows = rows;
        Total = total;
    }
}