using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Mkh;

/// <summary>
/// 树形结构返回模型
/// </summary>
/// <typeparam name="TKey">编号类型</typeparam>
/// <typeparam name="T"></typeparam>
public class TreeResultModel<TKey, T> where T : class, new()
{
    /// <summary>
    /// 编号
    /// </summary>
    public TKey Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 值(可选)
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// 扩展属性(可选)
    /// </summary>
    public string Extend { get; set; }
        
    /// <summary>
    /// 数据项
    /// </summary>
    public T Item { get; set; }

    /// <summary>
    /// 路径列表
    /// </summary>
    public List<string> Path { get; } = new();

    /// <summary>
    /// 子节点
    /// </summary>
    public List<TreeResultModel<TKey, T>> Children { get; set; } = new();
}

/// <summary>
/// 树形结构返回模型
/// </summary>
/// <typeparam name="T"></typeparam>
public class TreeResultModel<T> : TreeResultModel<int, T> where T : class, new()
{
    /// <summary>
    /// 子节点
    /// </summary>
    public new List<TreeResultModel<T>> Children { get; set; } = new();
}