using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mkh.Auth.Abstractions;
using Mkh.Auth.Jwt;
using Mkh.Cache.Abstractions;
using Mkh.Mod.Admin.Core.Domain.JwtAuthInfo;

namespace Mkh.Mod.Admin.Core.Infrastructure.Defaults
{
    internal class DefaultJwtTokenStorageProvider : IJwtTokenStorageProvider
    {
        private readonly IJwtAuthInfoRepository _repository;
        private readonly JwtOptions _jwtOptions;
        private readonly ICacheHandler _cacheHandler;
        private readonly AdminCacheKeys _cacheKeys;

        public DefaultJwtTokenStorageProvider(IJwtAuthInfoRepository repository, JwtOptions jwtOptions, ICacheHandler cacheHandler, AdminCacheKeys cacheKeys)
        {
            _repository = repository;
            _jwtOptions = jwtOptions;
            _cacheHandler = cacheHandler;
            _cacheKeys = cacheKeys;
        }

        public async Task Save(JwtResultModel model, List<Claim> claims)
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

            entity.LoginIP = claims.First(m => m.Type == MkhClaimTypes.IP).Value;
            entity.LoginTime = claims.First(m => m.Type == MkhClaimTypes.LOGIN_TIME).Value.ToLong();
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
            await _cacheHandler.Set(_cacheKeys.RefreshToken(model.AccountId, model.Platform), model.RefreshToken, entity.RefreshTokenExpiredTime);
        }

        public async Task<bool> Check(string refreshToken, Guid accountId, int platform)
        {
            var cacheToken = await _cacheHandler.Get(_cacheKeys.RefreshToken(accountId, platform));
            return cacheToken.NotNull() && cacheToken == refreshToken;
        }
    }
}
