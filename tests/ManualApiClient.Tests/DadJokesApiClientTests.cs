namespace ManualApiClient.Tests;

using ManualApiClient.Models;

public class DadJokesApiClientTests
{
    [Theory]
    [AutoData]
    public async Task SearchAsync_Returned(
        JokeSearchResponse response, string term, Uri host, string apiKey)
    {
        // Arrange
        var primaryHandler = CreateMessageHandlerWithResult(response);
        var httpClient = new HttpClient(primaryHandler.Object);
        var sut = DadJokesApiClientFactory.Create(httpClient, host.ToString(), apiKey);

        // Act
        var result = await sut.SearchAsync(term);

        // Assert
        result.Success.Should().Be(response.Success);
        result.Should().BeEquivalentTo(response);
        primaryHandler.Protected().Verify("SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(
                m => m.Headers.Accept.Select(h => h.MediaType).Contains("application/json")),
            ItExpr.IsAny<CancellationToken>());
    }

    [Theory]
    [AutoData]
    public async Task GetRandomJokeAsync_SingleJokeInResult_Returned(Joke joke)
    {
        // Arrange
        var response = new JokeSearchResponse
        {
            Success = true,
            Body = new() { joke }
        };
        var httpClient = CreateHttpClientWithResult(response);
        var sut = new DadJokesApiClient(httpClient);

        // Act
        var result = await sut.GetRandomJokeAsync();

        // Assert
        result.Should().BeEquivalentTo(joke);
    }

    [Theory]
    [AutoData]
    public async Task GetRandomJokeAsync_UnsuccessfulJokeResult_ExceptionThrown()
    {
        // Arrange
        var response = new JokeSearchResponse();
        var httpClient = CreateHttpClientWithResult(response);
        var sut = new DadJokesApiClient(httpClient);

        // Act
        // Assert
        await FluentActions.Invoking(() => sut.GetRandomJokeAsync())
            .Should().ThrowAsync<InvalidOperationException>();
    }
}
