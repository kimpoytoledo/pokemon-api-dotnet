using System.Threading.Tasks;
using PokemonAPI.Models;

namespace PokemonAPI.Services
{
    public interface IPokemonService
    {
        Task<PokemonInfo> GetPokemonDetailsAsync(string name);
    }
}
