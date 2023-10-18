using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PokemonAPI.Controllers;
using PokemonAPI.Services;
using PokemonAPI.Models;
using Xunit;

namespace PokemonAPITests
{
    public class PokemonControllerTests
    {
        [Fact]
        public async Task GetPokemon_ReturnsOkObjectResult()
        {
            // Arrange
            var pokemonServiceMock = new Mock<IPokemonService>();
            var loggerMock = new Mock<ILogger<PokemonController>>();
            var pokemonController = new PokemonController(pokemonServiceMock.Object, loggerMock.Object);

            var expectedPokemonInfo = new PokemonInfo
            {
                Id = 25,
                Name = "pikachu",
                Types = "electric"
            };
            pokemonServiceMock.Setup(p => p.GetPokemonDetailsAsync(It.IsAny<string>())).ReturnsAsync(expectedPokemonInfo);

            // Act
            var result = await pokemonController.GetPokemon("pikachu");

            // Assert
            var okResult = result as OkObjectResult;
            if (okResult != null)
            {
                Assert.Equal(expectedPokemonInfo, okResult.Value as PokemonInfo);
            }
            else
            {
                Assert.True(false, "Result is not an OkObjectResult");
            }
        }
    }
}
