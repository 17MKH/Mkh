using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Jwt;
using Mkh.Cache.Abstractions;
using Mkh.Mod.Admin.Core.Domain.JwtAuthInfo;
using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Infrastructure;

[ScopedInject]
internal class AdminJwtTokenStorage : IJwtTokenStorage
{
    private readonly IJwtAuthInfoRepository _repository;
    private readonly JwtOptions _jwtOptions;
    private readonly ICacheProvider _cacheHandler;
    private readonly AdminCacheKeys _cacheKeys;

    public AdminJwtTokenStorage(IJwtAuthInfoRepository repository, JwtOptions jwtOptions, ICacheProvider cacheHandler, AdminCacheKeys cacheKeys)
    {
        _repository = repository;
        _jwtOptions = jwtOptions;
        _cacheHandler = cacheHandler;
        _cacheKeys = cacheKeys;
    }

    public async Task Save(JwtCredential model, List<Claim> claims)
    {
        var platform = claims.First(m => m.Type == MkhClaimTypes.PLATFORM).Value.ToInt();
        var entity = await _repository.Find(m => m.AccountId == model.AccountId && m.Platform == platform)
            .ToFirst();

        var exists = true;
        if (entity == null)
        {
            exists = false;
            entity = new JwtAuthInfoEntity
            {
                AccountId = model.AccountId,
                Platform = platform
            };
        }

        entity.LoginIP = claims.First(m => m.Type == MkhClaimTypes.LOGIN_IP).Value;
        entity.LoginTime = model.LoginTime;
        entity.RefreshToken = model.RefreshToken;

        //默认刷新令牌有效期7天
        entity.RefreshTokenExpiredTime = DateTime.Now.AddDays(_jwtOptions.RefreshTokenExpires <= 0 ? 7 : _jwtOptions.RefreshTokenExpires);

        if (exists)
        {
            await _repository.Update(entity);
        }
        else
        {
            await _repository.Add(entity);
        }

        //刷新令牌加入缓存
        await _cacheHandler.Set(_cacheKeys.RefreshToken(model.RefreshToken, platform), model.AccountId, entity.RefreshTokenExpiredTime);
    }

    public async Task<Guid> CheckRefreshToken(string refreshToken, int platform)
    {
        return await _cacheHandler.Get<Guid>(_cacheKeys.RefreshToken(refreshToken, platform));
    }
}