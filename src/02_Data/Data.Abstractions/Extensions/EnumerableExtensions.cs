using System.Collections.Generic;

namespace Mkh.Data.Abstractions.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// 不包含，对应数据库中的Not In
    /// <para>此方法仅用于Mkh.Data中构造查询条件使用</para>
    /// </summary>
    /// <param name="list"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool NotContains<T>(this IEnumerable<T> list, T value)
    {
        return true;
    }
}