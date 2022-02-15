using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Mkh;

public static class EnumExtensions
{
    private static readonly ConcurrentDictionary<string, string> DescriptionCache = new();

    /// <summary>
    /// 包含UnKnown选项
    /// </summary>
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, OptionCollectionResultModel> ListCache = new();

    /// <summary>
    /// 不包含UnKnown选项
    /// </summary>
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, OptionCollectionResultModel> ListCacheNoIgnore = new();

    /// <summary>
    /// 获取枚举类型的Description说明
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToDescription(this Enum value)
    {
        var type = value.GetType();
        var info = type.GetField(value.ToString());
        var key = type.FullName + info.Name;
        if (!DescriptionCache.TryGetValue(key, out string desc))
        {
            var attrs = info.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs.Length < 1)
                desc = string.Empty;
            else
                desc = attrs[0] is DescriptionAttribute
                    descriptionAttribute
                    ? descriptionAttribute.Description
                    : value.ToString();

            DescriptionCache.TryAdd(key, desc);
        }

        return desc;
    }

    public static OptionCollectionResultModel ToResult(this Enum value, bool ignoreUnKnown = false)
    {
        var enumType = value.GetType();

        if (!enumType.IsEnum)
            return null;

        var options = Enum.GetValues(enumType).Cast<Enum>()
            .Where(m => !ignoreUnKnown || !m.ToString().Equals("UnKnown"));

        return Options2Collection(options);
    }

    /// <summary>
    /// 枚举转换为返回模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ignoreUnKnown">忽略UnKnown选项</param>
    /// <returns></returns>
    public static OptionCollectionResultModel ToResult<T>(bool ignoreUnKnown = false)
    {
        var enumType = typeof(T);

        if (!enumType.IsEnum)
            return null;

        if (ignoreUnKnown)
        {
            #region ==忽略UnKnown属性==

            if (!ListCacheNoIgnore.TryGetValue(enumType.TypeHandle, out OptionCollectionResultModel list))
            {
                var options = Enum.GetValues(enumType).Cast<Enum>()
                    .Where(m => !m.ToString().Equals("UnKnown"));

                list = Options2Collection(options);

                ListCacheNoIgnore.TryAdd(enumType.TypeHandle, list);
            }

            return list;

            #endregion ==忽略UnKnown属性==
        }
        else
        {
            #region ==包含UnKnown选项==

            if (!ListCache.TryGetValue(enumType.TypeHandle, out OptionCollectionResultModel list))
            {
                var options = Enum.GetValues(enumType).Cast<Enum>();
                list = Options2Collection(options);
                ListCache.TryAdd(enumType.TypeHandle, list);
            }

            return list;

            #endregion ==包含UnKnown选项==
        }
    }

    private static OptionCollectionResultModel Options2Collection(IEnumerable<Enum> options)
    {
        var collection = new OptionCollectionResultModel();
        foreach (var option in options)
        {
            collection.Add(new OptionResultModel
            {
                Label = option.ToDescription(),
                Value = option.ToInt()
            });
        }

        return collection;
    }
}