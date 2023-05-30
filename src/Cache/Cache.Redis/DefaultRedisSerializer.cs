using System;
using System.Text.Json;
using StackExchange.Redis;

namespace Mkh.Cache.Redis;

public class DefaultRedisSerializer : IRedisSerializer
{
    public string Serialize<T>(T value)
    {
        if (IsNotBaseType<T>())
        {
            return JsonSerializer.Serialize(value);
        }

        return value.ToString();
    }

    public T Deserialize<T>(RedisValue value)
    {
        if (IsNotBaseType<T>())
        {
            return JsonSerializer.Deserialize<T>(value);
        }

        return value.To<T>();
    }

    public object Deserialize(RedisValue value, Type type)
    {
        if (Type.GetTypeCode(type) == TypeCode.Object)
        {
            return JsonSerializer.Deserialize(value,type);
        }

        return value;
    }

    /// <summary>
    /// 是否是基础类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private bool IsNotBaseType<T>()
    {
        return Type.GetTypeCode(typeof(T)) == TypeCode.Object;
    }
}