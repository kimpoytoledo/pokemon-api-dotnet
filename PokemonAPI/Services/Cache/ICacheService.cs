using System.Threading.Tasks;
using PokemonAPI.Models;

namespace PokemonAPI.Services
{
    public interface ICacheService
    {
        Task<string> GetCachedResponseAsync(string key);
        Task SetCacheResponseAsync(string key, string response, int cacheDurationInSeconds);
    }
}
