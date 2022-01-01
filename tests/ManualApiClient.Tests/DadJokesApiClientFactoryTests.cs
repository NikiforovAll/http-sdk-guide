namespace ManualApiClient;

using ManualApiClient.Models;

public class DadJokesApiClientFactoryTests
{
    [Theory]
    [AutoData]
    public async Task Create_ChainOfDelegatingHandlers_Composed(JokeSearchResponse joke, Uri host, string apiKey)
    {
        var counterMock = new Mock<ICounter>(MockBehavior.Loose);
        var primaryHandler = CreateMessageHandlerWithResult(joke);
        var client = DadJokesApiClientFactory.Create(
            host.ToString(),
            apiKey,
            new CountingHandler() { Name = "handler1", Counter = counterMock.Object },
            new CountingHandler(primaryHandler.Object) { Name = "handler2", Counter = counterMock.Object });

        await client.GetRandomJokeAsync();

        counterMock.Verify(x => x.Count("handler1"), Times.Once());
        counterMock.Verify(x => x.Count("handler2"), Times.Once());
    }

    public interface ICounter
    {
        void Count(string countedBy);
    }

    private class CountingHandler : DelegatingHandler
    {
        public string Name { get; init; } = default!;
        public ICounter Counter { get; init; } = default!;

        public CountingHandler(HttpMessageHandler delegatingHandler)
            : base(delegatingHandler)
        {
        }

        public CountingHandler() { }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.Counter.Count(this.Name);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
