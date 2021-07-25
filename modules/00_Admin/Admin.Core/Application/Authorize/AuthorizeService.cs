using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Abstractions.Options;
using Mkh.Auth.Jwt;
using Mkh.Mod.Admin.Core.Application.Authorize.Dto;
using Mkh.Mod.Admin.Core.Domain.Account;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Models;

namespace Mkh.Mod.Admin.Core.Application.Authorize
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IOptionsMonitor<AuthOptions> _authOptions;
        private readonly IVerifyCodeProvider _verifyCodeProvider;
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHandler _passwordHandler;
        private readonly IAccountProfileResolver _accountProfileResolver;
        private readonly ICredentialClaimExtender _credentialClaimExtender;
        private readonly ICredentialBuilder _credentialBuilder;
        private readonly IJwtTokenStorageProvider _jwtTokenStorageProvider;

        public AuthorizeService(IOptionsMonitor<AuthOptions> authOptions, IVerifyCodeProvider verifyCodeProvider, IAccountRepository accountRepository, IPasswordHandler passwordHandler, IAccountProfileResolver accountProfileResolver, ICredentialClaimExtender credentialClaimExtender, ICredentialBuilder credentialBuilder, IJwtTokenStorageProvider jwtTokenStorageProvider)
        {
            _authOptions = authOptions;
            _verifyCodeProvider = verifyCodeProvider;
            _accountRepository = accountRepository;
            _passwordHandler = passwordHandler;
            _accountProfileResolver = accountProfileResolver;
            _credentialClaimExtender = credentialClaimExtender;
            _credentialBuilder = credentialBuilder;
            _jwtTokenStorageProvider = jwtTokenStorageProvider;
        }

        public async Task<IResultModel> Login(LoginDto dto)
        {
            var result = new ResultModel<AccountEntity>();

            //检测验证码
            if (_authOptions.CurrentValue.EnableVerifyCode)
            {
                var verifyCodeCheckResult = await _verifyCodeProvider.Verify(dto.VerifyCodeId, dto.VerifyCode);
                if (!verifyCodeCheckResult.Successful)
                    return result.Failed(verifyCodeCheckResult.Msg);
            }

            //查询账户
            var account = await _accountRepository.GetByUserName(dto.Username.FromBase64());
            if (account == null)
                return result.Failed("用户名或密码错误");

            //检测密码
            var password = _passwordHandler.Encrypt(dto.Password.FromBase64());
            if (!account.Password.Equals(password))
                return result.Failed("用户名或密码错误");

            if (account.Status == AccountStatus.Disabled)
                return result.Failed("账户已禁用，请联系管理员");

            //如果是未激活状态，则表示首次登录，需要将状态修改为激活
            if (account.Status == AccountStatus.Register)
            {
                await _accountRepository
                    .Find(m => m.Id == account.Id)
                    .ToUpdate(m => new AccountEntity
                    {
                        Status = AccountStatus.Active
                    });
            }

            var claims = new List<Claim>
            {
                new(MkhClaimTypes.TENANT_ID, account.TenantId != null ? account.TenantId.ToString() : ""),
                new(MkhClaimTypes.ACCOUNT_ID, account.Id.ToString()),
                new(MkhClaimTypes.ACCOUNT_NAME, account.Name),
                new(MkhClaimTypes.PLATFORM, dto.Platform.ToInt().ToString()),
                new(MkhClaimTypes.LOGIN_TIME, dto.LoginTime.ToString())
            };

            //验证IP
            if (_authOptions.CurrentValue.EnableCheckIP)
            {
                claims.Add(new(MkhClaimTypes.IP, dto.IP));
            }

            if (_credentialClaimExtender != null)
            {
                await _credentialClaimExtender.Extend(claims, account.Id);
            }

            return await _credentialBuilder.Build(claims);
        }

        public async Task<IResultModel> RefreshToken(RefreshTokenDto dto)
        {
            if (await _jwtTokenStorageProvider.Check(dto.RefreshToken, dto.AccountId, dto.Platform))
            {
                var account = await _accountRepository.Get(dto.AccountId);
                var claims = new List<Claim>
                {
                    new(MkhClaimTypes.TENANT_ID, account.TenantId != null ? account.TenantId.ToString() : ""),
                    new(MkhClaimTypes.ACCOUNT_ID, account.Id.ToString()),
                    new(MkhClaimTypes.ACCOUNT_NAME, account.Name),
                    new(MkhClaimTypes.PLATFORM, dto.Platform.ToInt().ToString()),
                    new(MkhClaimTypes.LOGIN_TIME, DateTime.Now.ToTimestamp().ToString()),
                    new(MkhClaimTypes.IP, dto.IP)
                };

                if (_credentialClaimExtender != null)
                {
                    await _credentialClaimExtender.Extend(claims, account.Id);
                }

                return await _credentialBuilder.Build(claims);
            }

            return ResultModel.Failed("令牌无效");
        }

        public async Task<IResultModel> GetProfile(Guid accountId, int platform)
        {
            var account = await _accountRepository.Get(accountId);
            if (account == null)
                return ResultModel.Failed("账户不存在");

            if (account.Status == AccountStatus.Disabled)
                return ResultModel.Failed("账户已禁用，请联系管理员");

            var profile = await _accountProfileResolver.Resolve(account, platform);

            return ResultModel.Success(profile);
        }
    }
}

