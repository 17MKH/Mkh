using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Abstractions.LoginHandlers;
using Mkh.Auth.Abstractions.Options;
using Mkh.Logging.Abstractions.Providers;
using Mkh.Mod.Admin.Core.Application.Account;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Application.Authorize;

/// <summary>
/// 用户名登录处理器
/// </summary>
[Scoped]
internal class UserNameLoginHandler : IUsernameLoginHandler
{
    private readonly IOptionsMonitor<AuthOptions> _authOptions;
    private readonly IVerifyCodeProvider _verifyCodeProvider;
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHandler _passwordHandler;
    private readonly ILoginLogProvider _loginLogProvider;
    private readonly IAccountService _accountService;
    private readonly IStringLocalizer<UserNameLoginHandler> _localizer;
    private readonly ITenantResolver _tenantResolver;

    public UserNameLoginHandler(IOptionsMonitor<AuthOptions> authOptions, IVerifyCodeProvider verifyCodeProvider, IAccountRepository accountRepository, IPasswordHandler passwordHandler, ILoginLogProvider loginLogProvider, IAccountService accountService, IStringLocalizer<UserNameLoginHandler> localizer, ITenantResolver tenantResolver)
    {
        _authOptions = authOptions;
        _verifyCodeProvider = verifyCodeProvider;
        _accountRepository = accountRepository;
        _passwordHandler = passwordHandler;
        _loginLogProvider = loginLogProvider;
        _accountService = accountService;
        _localizer = localizer;
        _tenantResolver = tenantResolver;
    }

    public async Task<IResultModel<UsernameLoginResult>> Handle(UsernameLoginModel model)
    {
        var result = new ResultModel<UsernameLoginResult>();
        var loginLog = new LoginLogModel
        {
            Username = model.Username,
            Password = model.Password,
            Platform = model.Platform
        };

        try
        {
            //检测验证码
            if (_authOptions.CurrentValue.EnableVerifyCode)
            {
                var verifyCodeCheckResult = await _verifyCodeProvider.Verify(model.VerifyCodeId, model.VerifyCode);
                if (!verifyCodeCheckResult.Successful)
                    return result.Failed(verifyCodeCheckResult.Msg);
            }

            //查询账户
            var msg = _localizer["The user name or password is incorrect"];
            string username;
            string password;
            try
            {
                username = model.Username.FromBase64();
                password = model.Password.FromBase64();
            }
            catch
            {
                return result.Failed(msg);
            }

            if (username.IsNull() || password.IsNull())
                return result.Failed(msg);

            //解析租户
            var tenantId = await _tenantResolver.Resolve();

            var account = await _accountRepository.GetByUserName(username, tenantId);
            if (account == null)
                return result.Failed(msg);

            password = _passwordHandler.Encrypt(model.Password.FromBase64());
            if (!account.Password.Equals(password))
                return result.Failed(msg);

            loginLog.AccountId = account.Id;

            //账户禁用
            if (account.Status == AccountStatus.Disabled)
                return result.Failed(_localizer["The account has been disabled. Please contact the administrator"]);

            //如果是未激活状态，则表示首次登录，需要将状态修改为激活
            if (account.Status == AccountStatus.Register)
            {
                await _accountService.Activate(account.Id);
            }

            return result.Success(new UsernameLoginResult
            {
                Platform = model.Platform,
                Username = model.Username,
                AccountId = account.Id,
                IP = model.IP,
                IPv4 = model.IPv4,
                IPv6 = model.IPv6,
                LoginTime = DateTime.Now.ToTimestamp(),
                TenantId = account.TenantId,
                UserAgent = model.Username
            });
        }
        finally
        {
            //记录日志
            await _loginLogProvider.Write(loginLog);
        }
    }
}