using Mkh.Data.Abstractions.Options;

namespace Mkh.Data.Abstractions.Logger;

/// <summary>
/// 数据库日志记录器
/// </summary>
public class DbLogger
{
    private readonly DbOptions _options;
    private readonly IDbLoggerProvider _provider;

    public DbLogger(DbOptions options, IDbLoggerProvider provider)
    {
        _options = options;
        _provider = provider;
    }

    /// <summary>
    /// 日志记录
    /// </summary>
    /// <param name="action">操作类型</param>
    /// <param name="sql">SQL语句</param>
    public void Write(string action, string sql)
    {
        if (_options.Log)
        {
            _provider.Write(action, sql);
        }
    }
}