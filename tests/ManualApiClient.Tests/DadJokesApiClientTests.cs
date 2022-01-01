namespace ManualApiClient.Tests;

using AutoFixture.Xunit2;
using FluentAssertions;
using ManualApiClient.Models;

public class DadJokesApiClientTests
{
    [Theory]
    [AutoData]
    public async Task SearchAsync_Returned(JokeSearchResponse response, string term)
    {
        // Arrange
        var httpClient = CreateHttpClientWithResult(response);
        var sut = new DadJokesApiClient(httpClient);

        // Act
        var result = await sut.SearchAsync(term);

        // Assert
        result.Success.Should().Be(response.Success);
        result.Should().BeEquivalentTo(response);
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
