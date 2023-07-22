using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Api.Repositories
{
    public interface IDistributedCacheWrapper
    {
        Task<string> GetStringAsync(string key);
        Task SetStringAsync(string key, string value);
        Task RemoveAsync(string key);
    }

    public class DistributedCacheWrapper : IDistributedCacheWrapper
    {
        private readonly IDistributedCache _redisCache;

        public DistributedCacheWrapper(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public Task<string> GetStringAsync(string key)
        {
            return _redisCache.GetStringAsync(key);
        }

        public Task SetStringAsync(string key, string value)
        {
            return _redisCache.SetStringAsync(key, value);
        }

        public Task RemoveAsync(string key)
        {
            return _redisCache.RemoveAsync(key);
        }
    }
}
