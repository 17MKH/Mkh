using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Abstractions.LoginHandlers;
using Mkh.Auth.Abstractions.Options;
using Mkh.Logging.Abstractions.Providers;
using Mkh.Mod.Admin.Core.Application.Account;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Infrastructure.LoginHandler;

/// <summary>
/// 用户名登录处理器
/// </summary>
[ScopedInject]
internal class UserNameLoginHandler : IUsernameLoginHandler
{
    private readonly IOptionsMonitor<AuthOptions> _authOptions;
    private readonly IVerifyCodeProvider _verifyCodeProvider;
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHandler _passwordHandler;
    private readonly ILoginLogHandler _loginLogProvider;
    private readonly IAccountService _accountService;
    private readonly ITenantResolver _tenantResolver;
    private readonly AdminLocalizer _localizer;

    public UserNameLoginHandler(IOptionsMonitor<AuthOptions> authOptions, IVerifyCodeProvider verifyCodeProvider, IAccountRepository accountRepository, IPasswordHandler passwordHandler, ILoginLogHandler loginLogProvider, IAccountService accountService, ITenantResolver tenantResolver, AdminLocalizer localizer)
    {
        _authOptions = authOptions;
        _verifyCodeProvider = verifyCodeProvider;
        _accountRepository = accountRepository;
        _passwordHandler = passwordHandler;
        _loginLogProvider = loginLogProvider;
        _accountService = accountService;
        _tenantResolver = tenantResolver;
        _localizer = localizer;
    }

    public async Task<IResultModel<UsernameLoginResult>> Handle(UsernameLoginModel model)
    {
        var result = new ResultModel<UsernameLoginResult>();
        var loginLog = new LoginLogModel
        {
            LoginMode = LoginMode.Username,
            Platform = model.Platform,
            LoginTime = DateTime.Now,
            UserAgent = model.UserAgent,
            IP = model.IP
        };

        try
        {
            //检测验证码
            if (_authOptions.CurrentValue.EnableVerifyCode)
            {
                var verifyCodeCheckResult = await _verifyCodeProvider.Verify(model.VerifyCodeId, model.VerifyCode);
                if (!verifyCodeCheckResult.Successful)
                {
                    loginLog.Error = verifyCodeCheckResult.Msg;
                    return result.Failed(verifyCodeCheckResult.Msg);
                }
            }

            //查询账户
            var msg = _localizer["用户名或密码错误"];
            string username = model.Username;
            string password = model.Password;

            if (_authOptions.CurrentValue.EncryptCert)
            {
                try
                {
                    username = model.Username.FromBase64();
                    password = model.Password.FromBase64();
                }
                catch
                {
                    loginLog.Error = msg;
                    return result.Failed(msg);
                }
            }

            if (username.IsNull() || password.IsNull())
            {
                loginLog.Error = msg;
                return result.Failed(msg);
            }

            //解析租户
            var tenantId = await _tenantResolver.ResolveId();

            loginLog.TenantId = tenantId;

            var account = await _accountRepository.GetByUserName(username, tenantId);
            if (account == null)
            {
                loginLog.Error = msg;
                return result.Failed(msg);
            }

            password = _passwordHandler.Encrypt(password);
            if (!account.Password.Equals(password))
            {
                loginLog.Error = msg;
                return result.Failed(msg);
            }

            loginLog.AccountId = account.Id;
            loginLog.AccountName = account.Name;

            //账户禁用
            if (account.Status == AccountStatus.Disabled)
            {
                msg = _localizer["账户已禁用，请联系管理员"];
                loginLog.Error = msg;
                return result.Failed(msg);
            }

            //如果是未激活状态，则表示首次登录，需要将状态修改为激活
            if (account.Status == AccountStatus.Register)
            {
                await _accountService.Activate(account.Id);
            }

            loginLog.Success = true;

            return result.Success(new UsernameLoginResult
            {
                AccountId = account.Id,
                Platform = model.Platform,
                Username = account.Username,
                AccountName = account.Name,
                IP = model.IP,
                IPv4 = model.IPv4,
                IPv6 = model.IPv6,
                LoginTime = DateTime.Now.ToTimestamp(),
                TenantId = account.TenantId,
                UserAgent = model.UserAgent
            });
        }
        finally
        {
            //记录登录日志
            if (_authOptions.CurrentValue.EnableLoginLog)
            {
                await _loginLogProvider.Write(loginLog);
            }
        }
    }
}