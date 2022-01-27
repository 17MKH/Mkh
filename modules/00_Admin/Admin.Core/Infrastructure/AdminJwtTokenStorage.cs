using System;
using System.Threading.Tasks;
using Mkh.Auth.Jwt;
using Mkh.Cache.Abstractions;
using Mkh.Mod.Admin.Core.Domain.JwtAuthInfo;

namespace Mkh.Mod.Admin.Core.Infrastructure;

internal class AdminJwtTokenStorage : IJwtTokenStorage
{
    private readonly IJwtAuthInfoRepository _repository;
    private readonly JwtOptions _jwtOptions;
    private readonly ICacheHandler _cacheHandler;
    private readonly AdminCacheKeys _cacheKeys;

    public AdminJwtTokenStorage(IJwtAuthInfoRepository repository, JwtOptions jwtOptions, ICacheHandler cacheHandler, AdminCacheKeys cacheKeys)
    {
        _repository = repository;
        _jwtOptions = jwtOptions;
        _cacheHandler = cacheHandler;
        _cacheKeys = cacheKeys;
    }

    public async Task Save(JwtCredential model)
    {
        var entity = await _repository.Find(m => m.AccountId == model.AccountId && m.Platform == model.Platform)
            .ToFirst();

        var exists = true;
        if (entity == null)
        {
            exists = false;
            entity = new JwtAuthInfoEntity
            {
                AccountId = model.AccountId,
                Platform = model.Platform
            };
        }

        entity.LoginIP = model.LoginIP;
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
        await _cacheHandler.Set(_cacheKeys.RefreshToken(model.RefreshToken, model.Platform), model.AccountId, entity.RefreshTokenExpiredTime);
    }

    public async Task<Guid> CheckRefreshToken(string refreshToken, int platform)
    {
        return await _cacheHandler.Get<Guid>(_cacheKeys.RefreshToken(refreshToken, platform));
    }
}