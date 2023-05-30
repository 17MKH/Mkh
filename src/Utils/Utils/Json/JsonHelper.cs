using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using Mkh.Utils.Annotations;
using Mkh.Utils.Json.Converters;

namespace Mkh.Utils.Json;

/// <summary>
/// JSON序列化帮助类
/// <para>该帮助类使用一组默认的JsonSerializerOptions配置来进行序列化/反序列化，配置如下：</para>
/// <para>1、不区分大小写的反序列化</para>
/// <para>2、属性名称使用 camel 大小写</para>
/// <para>3、最大限度减少字符转义</para>
/// <para>4、自定义日期转换器 DateTimeConverter</para>
/// </summary>
[SingletonInject]
public class JsonHelper
{
    private readonly JsonSerializerOptions _options = new();

    /// <summary>
    /// 静态实例
    /// </summary>
    public static readonly JsonHelper Instance = new();

    public JsonHelper()
    {
        //不区分大小写的反序列化
        _options.PropertyNameCaseInsensitive = true;
        //属性名称使用 camel 大小写
        _options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        //最大限度减少字符转义
        _options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        //添加日期转换器
        _options.Converters.Add(new DateTimeConverter());
        //添加多态嵌套序列化
        _options.AddPolymorphism();
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, typeof(T), _options);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _options);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="json">json文本</param>
    /// <param name="type">类型</param>
    /// <returns></returns>
    public object Deserialize(string json, Type type)
    {
        return JsonSerializer.Deserialize(json, type, _options);
    }
}