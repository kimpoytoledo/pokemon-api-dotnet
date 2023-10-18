using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using PokemonAPI.Models;
using PokemonAPI.Services;
using Xunit;
using Microsoft.Extensions.Configuration;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace PokemonAPITests
{
    public class PokemonServiceTests
    {
        [Fact]
        public async Task GetPokemonDetailsAsync_ReturnsExpectedPokemonInfo()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("{\"id\": 25, \"name\": \"pikachu\", \"types\": [{\"type\": {\"name\": \"electric\"}}]}"),
               });

            var httpClient = new HttpClient(handlerMock.Object);
            var cacheServiceMock = new Mock<ICacheService>();
            var loggerMock = new Mock<ILogger<PokemonService>>();
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.GetSection(It.IsAny<string>()).Value).Returns("3600");

            var pokemonService = new PokemonService(httpClient, cacheServiceMock.Object, configurationMock.Object, loggerMock.Object);

            // Act
            var result = await pokemonService.GetPokemonDetailsAsync("pikachu");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("pikachu", result.Name);
            Assert.Equal("electric", result.Types);
        }
    }
}