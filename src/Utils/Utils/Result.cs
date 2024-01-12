using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Mkh;

public abstract class ResultBase
{
    /// <summary>
    /// 成功的
    /// </summary>
    public bool Successful { get; protected set; }

    /// <summary>
    /// 错误码
    /// </summary>
    public string? ErrorCode { get; protected set; }

    /// <summary>
    /// 枚举类型的错误码
    /// </summary>
    internal Enum? ErrorCodeEnum { get; set; }

    /// <summary>
    /// 判断结果中的错误码是否与指定错误码相等
    /// </summary>
    /// <param name="errorCode"></param>
    /// <returns></returns>
    public bool EqualsErrorCode(string errorCode)
    {
        return errorCode.Equals(ErrorCode);
    }

    /// <summary>
    /// 判断结果中的枚举错误码是否与指定枚举错误码相等
    /// </summary>
    /// <param name="errorCodeEnum"></param>
    /// <returns></returns>
    public bool EqualsErrorCode(Enum errorCodeEnum)
    {
        return errorCodeEnum.Equals(ErrorCodeEnum);
    }
}

/// <summary>
/// 通用处理结果类
/// </summary>
public class Result : ResultBase
{
    /// <summary>
    /// 成功
    /// </summary>
    public Result Success()
    {
        Successful = true;
        return this;
    }

    /// <summary>
    /// 失败
    /// </summary>
    public Result Failed()
    {
        Successful = false;
        return this;
    }

    /// <summary>
    /// 失败
    /// </summary>
    /// <param name="errorCode">错误码</param>
    public Result Failed(string errorCode)
    {
        Successful = false;
        ErrorCode = errorCode;
        return this;
    }

    /// <summary>
    /// 失败
    /// </summary>
    /// <param name="errorCodeEnum">错误码枚举</param>
    public Result Failed(Enum errorCodeEnum)
    {
        Successful = false;
        ErrorCodeEnum = errorCodeEnum;
        return this;
    }
}

/// <summary>
/// 通用泛型处理结果类
/// </summary>
public class Result<T> : ResultBase
{
    /// <summary>
    /// 返回数据
    /// </summary>
    public T? Data { get; private set; }

    /// <summary>
    /// 成功
    /// </summary>
    public Result<T> Success(T? data)
    {
        Successful = true;
        Data = data;
        return this;
    }

    /// <summary>
    /// 失败的
    /// </summary>
    /// <returns></returns>
    public Result<T> Failed()
    {
        Successful = false;
        return this;
    }

    /// <summary>
    /// 失败的
    /// </summary>
    /// <param name="errorCode"></param>
    /// <returns></returns>
    public Result<T> Failed(string? errorCode)
    {
        Successful = false;
        ErrorCode = errorCode;
        return this;
    }

    /// <summary>
    /// 失败的
    /// </summary>
    /// <param name="errorCodeEnum">错误码枚举</param>
    /// <returns></returns>
    public Result<T> Failed(Enum errorCodeEnum)
    {
        Successful = false;
        ErrorCodeEnum = errorCodeEnum;
        return this;
    }
}

/// <summary>
/// 结果构造器
/// </summary>
public sealed class ResultBuilder
{
    /// <summary>
    /// 一个单例的状态为成功的结果实例
    /// </summary>
    private static readonly Result SuccessResult = new Result().Success();

    /// <summary>
    /// 一个单例的状态为失败的结果实例
    /// </summary>
    private static readonly Result FailedResult = new Result().Failed();

    /// <summary>
    /// 返回一个状态为成功的结果实例
    /// </summary>
    public static Result Success()
    {
        return SuccessResult;
    }

    /// <summary>
    /// 返回一个状态为失败的结果实例
    /// </summary>
    /// <returns></returns>
    public static Result Failed()
    {
        return FailedResult;
    }

    /// <summary>
    /// 返回一个状态为失败的结果实例
    /// </summary>
    /// <param name="errorCodeEnum">枚举类型的错误码</param>
    /// <returns></returns>
    public static Result Failed(Enum errorCodeEnum)
    {
        return new Result().Failed(errorCodeEnum);
    }

    /// <summary>
    /// 返回一个状态为失败的结果实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="errorCode">错误码</param>
    /// <returns></returns>
    public static Result<T> Failed<T>(string? errorCode = null)
    {
        return new Result<T>().Failed(errorCode);
    }

    /// <summary>
    /// 返回一个状态为失败的结果实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="errorCodeEnum">枚举类型的错误码</param>
    /// <returns></returns>
    public static Result<T> Failed<T>(Enum errorCodeEnum)
    {
        return new Result<T>().Failed(errorCodeEnum);
    }

    /// <summary>
    /// 返回一个状态为成功的结果实例
    /// </summary>
    public static Result<T> Success<T>(T? data = default)
    {
        return new Result<T>().Success(data);
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="task">任务</param>
    /// <returns></returns>
    public static async Task<Result<T>> SuccessAsync<T>(Task<T> task)
    {
        return new Result<T>().Success(await task);
    }

    /// <summary>
    /// 根据条件返回结果
    /// </summary>
    /// <param name="condition">判断条件</param>
    /// <returns></returns>
    public static Result Condition(bool condition)
    {
        return condition ? Success() : Failed();
    }

    /// <summary>
    /// 根据条件返回结果
    /// </summary>
    /// <param name="condition">判断条件</param>
    /// <returns></returns>
    public static async Task<Result> ConditionAsync(Task<bool> condition)
    {
        return Condition(await condition);
    }

    /// <summary>
    /// 根据条件返回结果
    /// </summary>
    /// <param name="condition">判断条件</param>
    /// <returns></returns>
    public static Result<T> Condition<T>(bool condition)
    {
        return condition ? Success<T>() : Failed<T>();
    }

    /// <summary>
    /// 根据条件返回结果
    /// </summary>
    /// <param name="condition">判断条件</param>
    /// <returns></returns>
    public static async Task<Result<T>> ConditionAsync<T>(Task<bool> condition)
    {
        return Condition<T>(await condition);
    }
}