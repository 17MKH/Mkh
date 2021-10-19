namespace Mkh.Cache.Redis;

/// <summary>
/// Redis配置项
/// </summary>
public class RedisOptions
{
    /// <summary>
    /// 默认数据库
    /// </summary>
    public int DefaultDb { get; set; }

    /// <summary>
    /// 缓存键前缀
    /// </summary>
    public string KeyPrefix { get; set; }

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = "127.0.0.1";
}