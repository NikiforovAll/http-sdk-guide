namespace ManualApiClient.Tests;

using FluentAssertions;
using ManualApiClient.Extensions;

public class TimeoutThrowingDelegatingHandlerTests
{
    [Fact]
    public async Task TimeOutExceptionThrown_ExceptionUnwrappedAndTimeoutExceptionThrown()
    {
        var httpHandlerChain = new TimeoutThrowingDelegatingHandler(new ThrowingOnTimeoutDelegatingHandler
        {
            TimeOut = TimeSpan.FromMilliseconds(100),
        });

        var httpClient = new HttpClient(httpHandlerChain)
        {
            BaseAddress = new Uri("https://test-timeout.com"),
        };

        await FluentActions.Invoking(() => httpClient.GetAsync("/"))
            .Should().ThrowAsync<TimeoutException>();
    }

    private class ThrowingOnTimeoutDelegatingHandler : HttpMessageHandler
    {
        public TimeSpan TimeOut { get; init; } = TimeSpan.Zero;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Task.Delay(this.TimeOut, cancellationToken);

            var message = "Test TimeoutException";
            throw new TaskCanceledException(message, new TimeoutException("Timeout"));
        }
    }
}
