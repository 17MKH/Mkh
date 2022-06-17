using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Abstractions.LoginHandlers;
using Mkh.Auth.Abstractions.Options;
using Mkh.Auth.Jwt;
using Mkh.Mod.Admin.Core.Application.Authorize.Dto;
using Mkh.Mod.Admin.Core.Application.Authorize.Vo;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Infrastructure;

namespace Mkh.Mod.Admin.Core.Application.Authorize;

public class AuthorizeService : IAuthorizeService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountProfileResolver _accountProfileResolver;
    private readonly ICredentialClaimExtender _credentialClaimExtender;
    private readonly ICredentialBuilder _credentialBuilder;
    private readonly IJwtTokenStorage _jwtTokenStorageProvider;
    private readonly IUsernameLoginHandler _usernameLoginHandler;
    private readonly IOptionsMonitor<AuthOptions> _authOptions;
    private readonly AdminLocalizer _localizer;

    public AuthorizeService(IAccountRepository accountRepository, IAccountProfileResolver accountProfileResolver, ICredentialClaimExtender credentialClaimExtender, ICredentialBuilder credentialBuilder, IJwtTokenStorage jwtTokenStorageProvider, IUsernameLoginHandler usernameLoginHandler, IOptionsMonitor<AuthOptions> authOptions, AdminLocalizer localizer)
    {
        _accountRepository = accountRepository;
        _accountProfileResolver = accountProfileResolver;
        _credentialClaimExtender = credentialClaimExtender;
        _credentialBuilder = credentialBuilder;
        _jwtTokenStorageProvider = jwtTokenStorageProvider;
        _usernameLoginHandler = usernameLoginHandler;
        _authOptions = authOptions;
        _localizer = localizer;
    }

    public async Task<IResultModel<ICredential>> UsernameLogin(UsernameLoginModel model)
    {
        var result = await _usernameLoginHandler.Handle(model);
        if (!result.Successful)
            return ResultModel.Failed<ICredential>(result.Msg);

        var loginResult = result.Data;

        var claims = new List<Claim>
        {
            new(MkhClaimTypes.TENANT_ID, loginResult.TenantId != null ? loginResult.TenantId.ToString() : ""),
            new(MkhClaimTypes.ACCOUNT_ID, loginResult.AccountId.ToString()),
            new(MkhClaimTypes.ACCOUNT_NAME, loginResult.AccountName),
            new(MkhClaimTypes.PLATFORM, model.Platform.ToString()),
            new(MkhClaimTypes.LOGIN_TIME, model.LoginTime.ToString())
        };

        //验证IP
        if (_authOptions.CurrentValue.EnableCheckIP)
        {
            claims.Add(new(MkhClaimTypes.LOGIN_IP, model.IP));
        }

        if (_credentialClaimExtender != null)
        {
            await _credentialClaimExtender.Extend(claims, loginResult.AccountId);
        }

        return ResultModel.Success(await _credentialBuilder.Build(claims));
    }

    public async Task<IResultModel<JwtCredential>> RefreshToken(RefreshTokenDto dto)
    {
        var accountId = await _jwtTokenStorageProvider.CheckRefreshToken(dto.RefreshToken, dto.Platform);
        if (accountId != Guid.Empty)
        {
            var account = await _accountRepository.Get(accountId);
            var claims = new List<Claim>
            {
                new(MkhClaimTypes.TENANT_ID, account.TenantId != null ? account.TenantId.ToString() : ""),
                new(MkhClaimTypes.ACCOUNT_ID, account.Id.ToString()),
                new(MkhClaimTypes.ACCOUNT_NAME, account.Name),
                new(MkhClaimTypes.PLATFORM, dto.Platform.ToInt().ToString()),
                new(MkhClaimTypes.LOGIN_TIME, DateTime.Now.ToTimestamp().ToString()),
                new(MkhClaimTypes.LOGIN_IP, dto.IP)
            };

            if (account.TenantId != null)
            {
            }

            if (_credentialClaimExtender != null)
            {
                await _credentialClaimExtender.Extend(claims, account.Id);
            }

            var jwtCredential = (JwtCredential)await _credentialBuilder.Build(claims);
            jwtCredential.RefreshToken = dto.RefreshToken;

            return ResultModel.Success(jwtCredential);
        }

        return ResultModel.Failed<JwtCredential>(_localizer["令牌无效"]);
    }

    public async Task<IResultModel<ProfileVo>> GetProfile(Guid accountId, int platform)
    {
        var account = await _accountRepository.Get(accountId);
        if (account == null)
            return ResultModel.Failed<ProfileVo>(_localizer["账户不存在"]);

        if (account.Status == AccountStatus.Disabled)
            return ResultModel.Failed<ProfileVo>(_localizer["账户已禁用，请联系管理员"]);

        var profile = await _accountProfileResolver.Resolve(account, platform);

        return ResultModel.Success(profile);
    }
}