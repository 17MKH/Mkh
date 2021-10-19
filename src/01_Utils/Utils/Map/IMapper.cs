namespace Mkh.Utils.Map;

/// <summary>
/// 对象映射器
/// </summary>
public interface IMapper
{
    /// <summary>
    /// 将对象映射为指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    T Map<T>(object source);

    /// <summary>
    /// 将对象映射为指定类型
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="source"></param>
    /// <param name="target"></param>
    void Map<TSource, TTarget>(TSource source, TTarget target);
}