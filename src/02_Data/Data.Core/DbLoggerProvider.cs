using Microsoft.Extensions.Logging;
using Mkh.Data.Abstractions.Logger;

namespace Mkh.Data.Core;

/// <summary>
/// 默认日志记录器
/// </summary>
internal class DbLoggerProvider : IDbLoggerProvider
{
    private readonly ILogger _logger;

    public DbLoggerProvider(ILogger<DbLoggerProvider> logger)
    {
        _logger = logger;
    }

    public void Write(string action, string sql)
    {
        _logger.LogInformation($"{action}:{sql}");
    }
}