using Library.Models;
using Library.Services.Interfaces;
using Library.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace LibraryTests.Services
{
    [TestClass]
    public class RandomUserServiceTests
    {
        private Mock<IConsoleService> _consoleServiceMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private RandomUserService _sut;

        [TestInitialize]
        public void Setup()
        {
            _consoleServiceMock = new Mock<IConsoleService>();

            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            _sut = new RandomUserService(_httpClient, _consoleServiceMock.Object);
        }

        [TestMethod]
        public async Task GetRandomDriverAsync_ShouldReturnDriver_WhenApiReturnsValidData()
        {
            // Arrange
            var expectedResponse = new RandomUserResponse
            {
                Results = new List<Result>
                {
                    new Result
                    {
                        Name = new Name
                        {
                            Title = "Mr",
                            First = "John",
                            Last = "Doe"
                        }
                    }
                }
            };
            var jsonResponse = JsonConvert.SerializeObject(expectedResponse);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse),
                });

            // Act
            var result = await _sut.GetRandomDriverAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Mr", result.Title);
            Assert.AreEqual("John", result.FirstName);
            Assert.AreEqual("Doe", result.LastName);
        }

        [TestMethod]
        public async Task GetRandomDriverAsync_ShouldReturnNullAndLogMessage_WhenNoResultsFound()
        {
            // Arrange
            var expectedResponse = new RandomUserResponse
            {
                Results = new List<Result>()
            };
            var jsonResponse = JsonConvert.SerializeObject(expectedResponse);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse),
                });

            // Act
            var result = await _sut.GetRandomDriverAsync();

            // Assert
            Assert.IsNull(result);
            _consoleServiceMock.Verify(cs => cs.WriteLine("No results found in the response."), Times.Once);
        }

        [TestMethod]
        public async Task GetRandomDriverAsync_ShouldReturnNullAndLogMessage_WhenHttpRequestExceptionOccurs()
        {
            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            var result = await _sut.GetRandomDriverAsync();

            // Assert
            Assert.IsNull(result);
            _consoleServiceMock.Verify(cs => cs.WriteLine(It.Is<string>(s => s.Contains("An error occurred while fetching data from the API"))), Times.Once);
        }

        [TestMethod]
        public async Task GetRandomDriverAsync_ShouldReturnNullAndLogMessage_WhenJsonSerializationExceptionOccurs()
        {
            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("Invalid JSON response"),
                });

            // Act
            var result = await _sut.GetRandomDriverAsync();

            // Assert
            Assert.IsNull(result);
            _consoleServiceMock.Verify(cs => cs.WriteLine(It.Is<string>(s => s.Contains("An error occurred while deserializing the response"))), Times.Once);
        }

        [TestMethod]
        public async Task GetRandomDriverAsync_ShouldReturnNullAndLogMessage_WhenGenericExceptionOccurs()
        {
            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _sut.GetRandomDriverAsync();

            // Assert
            Assert.IsNull(result);
            _consoleServiceMock.Verify(cs => cs.WriteLine(It.Is<string>(s => s.Contains("An unexpected error occurred"))), Times.Once);
        }
    }
}
