namespace ManualApiClient.Tests;

using System.Text.Json;

public static class TestHarness
{
    public static Mock<HttpMessageHandler> CreateMessageHandlerWithResult<T>(T result, HttpStatusCode code = HttpStatusCode.OK)
    {
        var messageHandler = new Mock<HttpMessageHandler>();
        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = code,
                Content = new StringContent(JsonSerializer.Serialize(result)),
            });

        return messageHandler;
    }

    public static HttpClient CreateHttpClientWithResult<T>(T result, HttpStatusCode code = HttpStatusCode.OK)
    {
        var httpClient = new HttpClient(CreateMessageHandlerWithResult(result, code).Object)
        {
            BaseAddress = new("https://api-client-under-test.com"),
        };

        return httpClient;
    }
}
