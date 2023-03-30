using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Mkh;

/// <summary>
/// 返回结果
/// </summary>
public class ResultModel<T> : IResultModel<T>
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
    public T Data { get; private set; }

    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="msg">说明</param>
    public ResultModel<T> Success(T data = default, string msg = "success")
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
    public ResultModel<T> Failed(string msg = "failed")
    {
        Successful = false;
        Msg = msg;
        return this;
    }

    public ResultModel()
    {
        Timestamp = DateTime.Now.ToTimestamp();
    }
}

/// <summary>
/// 返回结果
/// </summary>
public static partial class ResultModel
{
    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="data">返回数据</param>
    /// <returns></returns>
    public static IResultModel<T> Success<T>(T data = default)
    {
        return new ResultModel<T>().Success(data);
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <returns></returns>
    public static IResultModel Success()
    {
        return Success<string>();
    }

    /// <summary>
    /// 失败
    /// </summary>
    /// <param name="error">错误信息</param>
    /// <returns></returns>
    public static IResultModel<T> Failed<T>(string error = null)
    {
        return new ResultModel<T>().Failed(error ?? "failed");
    }

    /// <summary>
    /// 失败
    /// </summary>
    /// <returns></returns>
    public static IResultModel Failed(string error = null)
    {
        return Failed<string>(error);
    }

    /// <summary>
    /// 根据布尔值返回结果
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static IResultModel<T> Result<T>(bool success)
    {
        return success ? Success<T>() : Failed<T>();
    }

    /// <summary>
    /// 根据布尔值返回结果
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static async Task<IResultModel> Result(Task<bool> success)
    {
        return await success ? Success() : Failed();
    }

    /// <summary>
    /// 根据布尔值返回结果
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static IResultModel Result(bool success)
    {
        return success ? Success() : Failed();
    }

    /// <summary>
    /// 数据已存在
    /// </summary>
    /// <returns></returns>
    public static IResultModel HasExists => Failed("data already exists");

    /// <summary>
    /// 数据不存在
    /// </summary>
    public static IResultModel NotExists => Failed("data doesn't exist");
}