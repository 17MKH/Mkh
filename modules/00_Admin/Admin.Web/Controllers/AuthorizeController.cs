using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Abstractions.Annotations;
using Mkh.Auth.Abstractions.LoginHandlers;
using Mkh.Auth.Jwt;
using Mkh.Mod.Admin.Core.Application.Authorize;
using Mkh.Mod.Admin.Core.Application.Authorize.Dto;
using Mkh.Mod.Admin.Core.Application.Authorize.Vo;
using Mkh.Mod.Admin.Core.Infrastructure;
using Mkh.Utils.Web;

namespace Mkh.Mod.Admin.Web.Controllers;

/// <summary>
/// 身份认证
/// </summary>
public class AuthorizeController : Web.ModuleController
{
    private readonly IAuthorizeService _service;
    private readonly IPResolver _ipResolver;
    private readonly IVerifyCodeProvider _verifyCodeProvider;
    private readonly IAccount _account;

    public AuthorizeController(IAuthorizeService service, IPResolver ipResolver, IVerifyCodeProvider verifyCodeProvider, IAccount account)
    {
        _service = service;
        _ipResolver = ipResolver;
        _verifyCodeProvider = verifyCodeProvider;
        _account = account;
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IResultModel> VerifyCode()
    {
        return ResultModel.Success(await _verifyCodeProvider.Create());
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [DisableAudit]
    public Task<IResultModel<ICredential>> Login(UsernameLoginModel model)
    {
        model.IP = _ipResolver.IP;
        model.IPv4 = _ipResolver.IPv4;
        model.IPv6 = _ipResolver.IPv6;
        model.UserAgent = _ipResolver.UserAgent;
        model.LoginTime = DateTime.Now.ToTimestamp();

        return _service.UsernameLogin(model);
    }

    /// <summary>
    /// 刷新令牌
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public Task<IResultModel<JwtCredential>> RefreshToken(RefreshTokenDto dto)
    {
        dto.IP = _ipResolver.IP;
        return _service.RefreshToken(dto);
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowWhenAuthenticated]
    public Task<IResultModel<ProfileVo>> Profile()
    {
        return _service.GetProfile(_account.Id, _account.Platform);
    }
}