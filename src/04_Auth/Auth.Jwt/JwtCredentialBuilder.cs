using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Mkh.Auth.Abstractions;
using Mkh.Utils.Models;

namespace Mkh.Auth.Jwt
{
    /// <summary>
    /// JWT凭证生成器
    /// </summary>
    public class JwtCredentialBuilder : ICredentialBuilder
    {
        private readonly JwtOptions _options;
        private readonly ILogger<JwtCredentialBuilder> _logger;

        public JwtCredentialBuilder(JwtOptions options, ILogger<JwtCredentialBuilder> logger)
        {
            _options = options;
            _logger = logger;
        }

        public IResultModel Build(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, DateTime.Now, DateTime.Now.AddMinutes(_options.Expires), signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            _logger.LogDebug("生成AccessToken：{token}", token);

            return ResultModel.Success(new
            {
                AccessToken = token,
                ExpiresIn = (_options.Expires < 0 ? 120 : _options.Expires) * 60,
                RefreshToken = Guid.NewGuid().ToString().Replace("-", "")
            });
        }
    }
}
