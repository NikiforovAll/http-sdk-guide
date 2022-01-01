namespace ManualApiClient.Tests;

using HeaderPropagation;
using ManualApiClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

public class HeaderPropagationMessageHandlerTests
{
    [Theory]
    [AutoData]
    public async Task HeaderProvidedInContextAccessor_PassedThroughHttpClient(
        JokeSearchResponse response, Uri host, string apiKey)
    {
        // Arrange
        var correlationHeader = "X-Correlation-Id";
        var correlationId = "b5a95a63.f190.4c91.880a.5b54512fa6b1";
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var context = new DefaultHttpContext();
        context.Request.Headers[correlationHeader] = correlationId;
        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

        var httpHandlerChain = new HeaderPropagationMessageHandler(
            new HeaderPropagationOptions { HeaderNames = { correlationHeader } },
            mockHttpContextAccessor.Object,
            NullLogger<HeaderPropagationMessageHandler>.Instance);

        var primaryHandler = CreateMessageHandlerWithResult(response);
        httpHandlerChain.InnerHandler = primaryHandler.Object;
        var httpClient = new HttpClient(httpHandlerChain);
        var sut = DadJokesApiClientFactory.Create(httpClient, host.ToString(), apiKey);

        // Act
        var result = await sut.GetRandomJokeAsync();

        // Assert
        primaryHandler.Protected().Verify("SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(m => m.Headers.Contains(correlationHeader)),
            ItExpr.IsAny<CancellationToken>());
    }
}
