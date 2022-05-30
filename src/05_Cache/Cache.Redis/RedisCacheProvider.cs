using System;
using System.Threading.Tasks;
using Mkh.Cache.Abstractions;

namespace Mkh.Cache.Redis;

public class RedisCacheProvider : ICacheProvider
{
    private readonly RedisHelper _helper;

    public RedisCacheProvider(RedisHelper helper)
    {
        _helper = helper;
    }

    public Task<string> Get(string key)
    {
        return _helper.StringGetAsync<string>(key);
    }

    public Task<T> Get<T>(string key)
    {
        return _helper.StringGetAsync<T>(key);
    }

    public Task<bool> Set<T>(string key, T value)
    {
        return _helper.StringSetAsync(key, value);
    }

    public Task<bool> Set<T>(string key, T value, int expires)
    {
        return _helper.StringSetAsync(key, value, new TimeSpan(0, 0, expires, 0));
    }

    public Task<bool> Set<T>(string key, T value, DateTime expires)
    {
        return _helper.StringSetAsync(key, value, expires - DateTime.Now);
    }

    public Task<bool> Set<T>(string key, T value, TimeSpan expires)
    {
        return _helper.StringSetAsync(key, value, expires);
    }

    public Task<bool> Remove(string key)
    {
        return _helper.KeyDeleteAsync(key);
    }

    public Task<bool> Exists(string key)
    {
        return _helper.KeyExistsAsync(key);
    }

    public Task RemoveByPrefix(string prefix)
    {
        return _helper.DeleteByPrefix(prefix);
    }
}