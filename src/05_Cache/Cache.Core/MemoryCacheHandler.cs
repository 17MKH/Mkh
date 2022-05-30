using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Mkh.Cache.Abstractions;

namespace Mkh.Cache.Core;

public class MemoryCacheHandler : ICacheProvider
{
    private readonly IMemoryCache _cache;

    public MemoryCacheHandler(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<string> Get(string key)
    {
        var value = _cache.Get(key);
        return Task.FromResult(value?.ToString());
    }

    public Task<T> Get<T>(string key)
    {
        return Task.FromResult(_cache.Get<T>(key));
    }

    public Task<bool> Set<T>(string key, T value)
    {
        _cache.Set(key, value);
        return Task.FromResult(true);
    }

    public Task<bool> Set<T>(string key, T value, int expires)
    {
        _cache.Set(key, value, new TimeSpan(0, 0, expires, 0));
        return Task.FromResult(true);
    }

    public Task<bool> Set<T>(string key, T value, DateTime expires)
    {
        _cache.Set(key, value, expires - DateTime.Now);
        return Task.FromResult(true);
    }

    public Task<bool> Set<T>(string key, T value, TimeSpan expires)
    {
        _cache.Set(key, value, expires);
        return Task.FromResult(true);
    }

    public Task<bool> Remove(string key)
    {
        _cache.Remove(key);
        return Task.FromResult(true);
    }

    public Task<bool> Exists(string key)
    {
        return Task.FromResult(_cache.TryGetValue(key, out _));
    }

    public async Task RemoveByPrefix(string prefix)
    {
        if (prefix.IsNull())
            return;

        var keys = GetAllKeys().Where(m => m.StartsWith(prefix));
        foreach (var key in keys)
        {
            await Remove(key);
        }
    }

    private List<string> GetAllKeys()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
        var cacheItems = entries as IDictionary;
        var keys = new List<string>();
        if (cacheItems == null) return keys;
        foreach (DictionaryEntry cacheItem in cacheItems)
        {
            keys.Add(cacheItem.Key.ToString());
        }
        return keys;
    }
}