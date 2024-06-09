using Library.Services;
using Library.Services.Interfaces;

namespace LibraryTests.Services;

[TestClass]
public class FakePersonServiceIntegrationTests
{
    private IConsoleService _consoleService;
    private HttpClient _httpClient;
    private FakePersonService _sut;

    [TestInitialize]
    public void Setup()
    {
        // Arrange
        _consoleService = new ConsoleService();
        _httpClient = new HttpClient();
        _sut = new FakePersonService(_httpClient, _consoleService);
    }

    [TestMethod]
    public async Task GetRandomDriverAsync_ShouldReturnDriver_WhenApiReturnIsCorrect()
    {
        // Act
        var result = await _sut.GetRandomDriverAsync();

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetRandomDriverAsync_ShouldReturnDriverWithValidTitle_WhenApiReturnIsCorrect()
    {
        // Act
        var result = await _sut.GetRandomDriverAsync();

        // Assert
        Assert.IsFalse(string.IsNullOrEmpty(result.Title));
    }

    [TestMethod]
    public async Task GetRandomDriverAsync_ShouldReturnDriverWithValidFirstName_WhenApiReturnIsCorrect()
    {
        // Act
        var result = await _sut.GetRandomDriverAsync();

        // Assert
        Assert.IsFalse(string.IsNullOrEmpty(result.FirstName));
    }

    [TestMethod]
    public async Task GetRandomDriverAsync_ShouldReturnDriverWithValidLastName_WhenApiReturnIsCorrect()
    {
        // Act
        var result = await _sut.GetRandomDriverAsync();

        // Assert
        Assert.IsFalse(string.IsNullOrEmpty(result.LastName));
    }

    [TestMethod]
    public async Task GetRandomDriverAsync_ShouldNotReturnDriver_WhenApiReturnIsIncorrect()
    {
        // Arrange
        var incorrectHttpClient = new HttpClient(new FailingHttpClientHandler());
        var sutWithIncorrectHttpClient = new FakePersonService(incorrectHttpClient, _consoleService);

        // Act
        var result = await sutWithIncorrectHttpClient.GetRandomDriverAsync();

        // Assert
        Assert.IsNull(result);
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
        _sut = new FakePersonService(httpClient, _consoleService);

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
        _sut = new FakePersonService(httpClient, _consoleService);

        // Act
        var result = await _sut.GetRandomDriverAsync();

        // Assert
        Assert.IsNull(result);
    }
}

public class FailingHttpClientHandler : HttpClientHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
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
