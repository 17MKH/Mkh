using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mkh.Auth.Abstractions;
using Mkh.Mod.Admin.Core.Application.Authorize;
using Mkh.Mod.Admin.Core.Application.Authorize.Dto;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Models;
using Mkh.Utils.Web;

namespace Mkh.Mod.Admin.Web.Controllers
{
    /// <summary>
    /// 身份认证
    /// </summary>
    public class AuthorizeController : ModuleController
    {
        private readonly IAuthorizeService _service;
        private readonly IPResolver _ipResolver;
        private readonly ICredentialBuilder _credentialBuilder;
        private readonly ICredentialClaimExtender _credentialClaimExtender;
        private readonly IVerifyCodeProvider _verifyCodeProvider;
        private readonly IAccount _account;

        public AuthorizeController(IAuthorizeService service, IPResolver ipHelper, ICredentialBuilder credentialBuilder, ICredentialClaimExtender credentialClaimExtender, IVerifyCodeProvider verifyCodeProvider, IAccount account)
        {
            _service = service;
            _ipResolver = ipHelper;
            _credentialBuilder = credentialBuilder;
            _credentialClaimExtender = credentialClaimExtender;
            _verifyCodeProvider = verifyCodeProvider;
            _account = account;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Task<IResultModel> VerifyCode()
        {
            return ResultModel.SuccessAsync(_verifyCodeProvider.Create());
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResultModel> Login(LoginDto dto)
        {
            dto.IPv4 = _ipResolver.IPv4;
            dto.IPv6 = _ipResolver.IPv6;
            dto.UserAgent = _ipResolver.UserAgent;
            dto.LoginTime = DateTime.Now.ToTimestamp();

            var loginResult = await _service.Login(dto);
            if (loginResult.Successful)
            {
                var account = loginResult.Data;
                var claims = new List<Claim>
                {
                    new(MkhClaimTypes.TENANT_ID, account.TenantId != null ? account.TenantId.ToString() : ""),
                    new(MkhClaimTypes.ACCOUNT_ID, account.Id.ToString()),
                    new(MkhClaimTypes.ACCOUNT_NAME, account.Name),
                    new(MkhClaimTypes.PLATFORM, dto.Platform.ToInt().ToString()),
                    new(MkhClaimTypes.LOGIN_TIME, dto.LoginTime.ToString())
                };

                if (_credentialClaimExtender != null)
                {
                    await _credentialClaimExtender.Extend(claims, account.Id);
                }

                return _credentialBuilder.Build(claims);
            }

            return loginResult;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IResultModel> Profile()
        {
            return _service.GetProfile(_account.AccountId);
        }
    }
}
