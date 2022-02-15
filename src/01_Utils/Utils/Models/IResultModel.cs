// ReSharper disable once CheckNamespace

using Mkh.Utils.Json.Converters;

namespace Mkh;

/// <summary>
/// 返回结果模型接口
/// </summary>
[JsonPolymorphism]
public interface IResultModel
{
    /// <summary>
    /// 是否成功
    /// </summary>
    bool Successful { get; }

    /// <summary>
    /// 错误信息
    /// </summary>
    string Msg { get; }

    /// <summary>
    /// 业务码，用于业务中自定义
    /// </summary>
    string Code { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    long Timestamp { get; }
}

/// <summary>
/// 返回结果模型泛型接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResultModel<out T> : IResultModel
{
    /// <summary>
    /// 返回数据
    /// </summary>
    T Data { get; }
}