using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Services;
using PokemonAPI.Models;
using System.Threading.Tasks;

namespace PokemonAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private readonly ILogger<PokemonController> _logger; 

        public PokemonController(IPokemonService pokemonService, ILogger<PokemonController> logger)
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPokemon(string name)
        {
            try
            {
                var pokemonInfo = await _pokemonService.GetPokemonDetailsAsync(name);
                return Ok(pokemonInfo);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Application error while fetching details for Pokemon {PokemonName}", name);
                return StatusCode(502);  // Bad Gateway, indicating an issue with an upstream service
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching details for Pokemon {PokemonName}", name);
                return StatusCode(500);  // Internal Server Error
            }
        }
    }
}
