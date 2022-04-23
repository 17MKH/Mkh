using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mkh.Logging.Abstractions.Providers;

namespace Mkh.Logging.Core.DefaultProviders;

/// <summary>
/// 默认登录日志处理器
/// </summary>
public class LoginLogProvider : ILoginLogProvider
{
    private readonly ILogger<LoginLogProvider> _logger;

    public LoginLogProvider(ILogger<LoginLogProvider> logger)
    {
        _logger = logger;
    }

    public Task<bool> Write(LoginLogModel model)
    {
        _logger.LogInformation("Login Log：{@model}", model);

        return Task.FromResult(true);
    }
}