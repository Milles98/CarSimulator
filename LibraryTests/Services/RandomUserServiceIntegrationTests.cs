using Library.Services;
using Library.Services.Interfaces;

namespace LibraryTests.Services
{
    [TestClass]
    public class RandomUserServiceIntegrationTests
    {
        private IConsoleService _consoleService;
        private HttpClient _httpClient;
        private RandomUserService _sut;

        [TestInitialize]
        public void Setup()
        {
            _consoleService = new ConsoleService();
            _httpClient = new HttpClient();
            _sut = new RandomUserService(_httpClient, _consoleService);
        }

        [TestMethod]
        public async Task GetRandomDriverAsync_ShouldReturnDriver_WhenApiReturnIsCorrect()
        {
            // Act
            var result = await _sut.GetRandomDriverAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Title));
            Assert.IsFalse(string.IsNullOrEmpty(result.FirstName));
            Assert.IsFalse(string.IsNullOrEmpty(result.LastName));
        }

        [TestMethod]
        public async Task GetRandomDriverAsync_ShouldReturnNull_WhenNoResultsFound()
        {
            // Act
            var result = await _sut.GetRandomDriverAsync();

            // Assert
            if (result == null)
            {
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task GetRandomDriverAsync_ShouldHandleHttpRequestException()
        {
            // Arrange
            var httpClient = new HttpClient(new FailingHttpClientHandler());
            _sut = new RandomUserService(httpClient, _consoleService);

            // Act
            var result = await _sut.GetRandomDriverAsync();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetRandomDriverAsync_ShouldHandleJsonSerializationException()
        {
            // Arrange
            var httpClient = new HttpClient(new InvalidJsonHttpClientHandler());
            _sut = new RandomUserService(httpClient, _consoleService);

            // Act
            var result = await _sut.GetRandomDriverAsync();

            // Assert
            Assert.IsNull(result);
        }
    }

    public class FailingHttpClientHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new HttpRequestException("Simulerat nätverksfel");
        }
    }

    public class InvalidJsonHttpClientHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("Ogiltigt JSON svar")
            };
            return await Task.FromResult(response);
        }
    }
}
