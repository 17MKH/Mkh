using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mkh.Auth.Jwt;

/// <summary>
/// JWT令牌存储器
/// </summary>
public interface IJwtTokenStorage
{
    /// <summary>
    /// 存储
    /// </summary>
    /// <param name="model"></param>
    /// <param name="claims"></param>
    /// <returns></returns>
    Task Save(JwtCredential model, List<Claim> claims);

    /// <summary>
    /// 检测刷新令牌并返回账户编号
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    Task<Guid> CheckRefreshToken(string refreshToken, int platform);
}

internal class DefaultJwtTokenStorage : IJwtTokenStorage
{
    public Task Save(JwtCredential model, List<Claim> claims)
    {
        return Task.CompletedTask;
    }

    public Task<Guid> CheckRefreshToken(string refreshToken, int platform)
    {
        return Task.FromResult(Guid.Empty);
    }
}