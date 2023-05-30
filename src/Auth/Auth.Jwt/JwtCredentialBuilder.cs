using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Mkh.Auth.Abstractions;

namespace Mkh.Auth.Jwt;

/// <summary>
/// JWT凭证生成器
/// </summary>
public class JwtCredentialBuilder : ICredentialBuilder
{
    private readonly JwtOptions _options;
    private readonly ILogger<JwtCredentialBuilder> _logger;
    private readonly IJwtTokenStorage _jwtTokenStorage;

    public JwtCredentialBuilder(JwtOptions options, ILogger<JwtCredentialBuilder> logger, IJwtTokenStorage tokenStorage)
    {
        _options = options;
        _logger = logger;
        _jwtTokenStorage = tokenStorage;
    }

    public async Task<ICredential> Build(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, DateTime.Now, DateTime.Now.AddMinutes(_options.Expires), signingCredentials);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        _logger.LogDebug("build access_token：{token}", token);

        var jwtCredential = new JwtCredential
        {
            AccountId = Guid.Parse(claims.First(m => m.Type == MkhClaimTypes.ACCOUNT_ID).Value),
            LoginTime = claims.First(m => m.Type == MkhClaimTypes.LOGIN_TIME).Value.ToLong(),
            AccessToken = token,
            ExpiresIn = (_options.Expires < 0 ? 120 : _options.Expires) * 60,
            RefreshToken = Guid.NewGuid().ToString().Replace("-", "")
        };

        //存储令牌信息
        await _jwtTokenStorage.Save(jwtCredential, claims);

        return jwtCredential;
    }
}