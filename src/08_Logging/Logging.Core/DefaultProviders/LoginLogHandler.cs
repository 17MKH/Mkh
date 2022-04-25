using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mkh.Logging.Abstractions.Providers;

namespace Mkh.Logging.Core.DefaultProviders;

/// <summary>
/// 默认登录日志处理器
/// </summary>
internal class LoginLogHandler : ILoginLogHandler
{
    private readonly ILogger<LoginLogHandler> _logger;

    public LoginLogHandler(ILogger<LoginLogHandler> logger)
    {
        _logger = logger;
    }

    public Task<bool> Write(LoginLogModel model)
    {
        _logger.LogInformation("Login Log：{@model}", model);

        return Task.FromResult(true);
    }
}