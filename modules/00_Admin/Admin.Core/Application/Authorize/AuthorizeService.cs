using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mkh.Auth.Abstractions.Options;
using Mkh.Mod.Admin.Core.Application.Authorize.Dto;
using Mkh.Mod.Admin.Core.Application.Authorize.Vo;
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

        public AuthorizeService(IOptionsMonitor<AuthOptions> authOptions, IVerifyCodeProvider verifyCodeProvider, IAccountRepository accountRepository, IPasswordHandler passwordHandler)
        {
            _authOptions = authOptions;
            _verifyCodeProvider = verifyCodeProvider;
            _accountRepository = accountRepository;
            _passwordHandler = passwordHandler;
        }

        public async Task<IResultModel<AccountEntity>> Login(LoginDto dto)
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
            var account = await _accountRepository.GetByUserName(dto.Username);
            if (account == null)
                return result.Failed("用户名或密码错误");

            //检测密码
            var password = _passwordHandler.Encrypt(dto.Password);
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

            return result.Success(account);
        }

        public async Task<IResultModel> GetProfile(Guid accountId)
        {
            var account = await _accountRepository.Get(accountId);
            if (account == null)
                return ResultModel.Failed("账户不存在");

            if (account.Status == AccountStatus.Disabled)
                return ResultModel.Failed("账户已禁用，请联系管理员");

            var vo = new ProfileVo
            {
                AccountId = accountId,
                Avatar = account.Avatar,
                Username = account.Username,
                Name = account.Name,
                Phone = account.Phone,
                Email = account.Email,
            };

            return ResultModel.Success(vo);
        }
    }
}
