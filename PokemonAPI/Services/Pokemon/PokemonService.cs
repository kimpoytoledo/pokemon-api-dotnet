using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PokemonAPI.Models;

namespace PokemonAPI.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        private readonly int _cacheDurationInSeconds;
        private readonly ILogger<PokemonService> _logger;

        public PokemonService(HttpClient httpClient, ICacheService cacheService, IConfiguration configuration, ILogger<PokemonService> logger)
        {
            _httpClient = httpClient;
            _cacheService = cacheService;
            _cacheDurationInSeconds = int.Parse(configuration.GetSection("Redis:CacheDuration").Value);
            _logger = logger;
        }

        public async Task<PokemonInfo> GetPokemonDetailsAsync(string name)
        {
            try
            {
                var cachedResponse = await _cacheService.GetCachedResponseAsync(name);
                if (!string.IsNullOrEmpty(cachedResponse))
                {
                    var cachedPokemonInfo = JsonSerializer.Deserialize<PokemonInfo>(cachedResponse);
                    if (cachedPokemonInfo != null)
                    {
                        cachedPokemonInfo.Source = "Redis Cache";
                        return cachedPokemonInfo;
                    }
                }

                var response = await _httpClient.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{name}");
                var pokemonInfo = ExtractPokemonInfo(response);
                pokemonInfo.Source = "PokéAPI";
                var jsonPokemonInfo = JsonSerializer.Serialize(pokemonInfo);
                await _cacheService.SetCacheResponseAsync(name, jsonPokemonInfo,_cacheDurationInSeconds);
                return pokemonInfo;
}
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while calling the PokeAPI for Pokemon {PokemonName}", name);
                throw new ApplicationException("External API error", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "An error occurred while parsing the API response for Pokemon {PokemonName}", name);
                throw new ApplicationException("Response parsing error", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching details for Pokemon {PokemonName}", name);
                throw;
            }
        }

        private PokemonInfo ExtractPokemonInfo(string json)
        {
            using var jsonDoc = JsonDocument.Parse(json);
            var rootElement = jsonDoc.RootElement;
            var id = rootElement.GetProperty("id").GetInt32();
            var name = rootElement.GetProperty("name").GetString();
            var typesArray = rootElement.GetProperty("types");
            var types = "";
            foreach (var typeElement in typesArray.EnumerateArray())
            {
                var type = typeElement.GetProperty("type").GetProperty("name").GetString();
                types += string.IsNullOrEmpty(types) ? type : $", {type}";
            }
            return new PokemonInfo { Id = id, Name = name, Types = types };
        }
    }
}
