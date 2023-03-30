namespace Mkh.Data.Abstractions.Logger;

/// <summary>
/// 数据库日志提供器
/// </summary>
public interface IDbLoggerProvider
{
    /// <summary>
    /// 日志记录
    /// </summary>
    /// <param name="action">操作类型</param>
    /// <param name="sql">SQL语句</param>
    void Write(string action, string sql);
}