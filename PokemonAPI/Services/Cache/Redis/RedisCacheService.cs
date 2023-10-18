using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokemonAPI.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            var cachedResponse = await _cache.GetStringAsync(key);
            return cachedResponse;
        }

        public async Task SetCacheResponseAsync(string key, string response, int cacheDurationInSeconds)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheDurationInSeconds)
            };
            await _cache.SetStringAsync(key, response, options);
        }
    }
}
