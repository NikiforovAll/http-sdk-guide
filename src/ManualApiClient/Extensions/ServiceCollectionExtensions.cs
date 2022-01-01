namespace ManualApiClient.Extensions;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddDadJokesApiClient(
        this IServiceCollection services,
        Action<HttpClient> configureClient) =>
            services.AddHttpClient<IDadJokesApiClient, DadJokesApiClient>((httpClient) =>
            {
                DadJokesApiClientFactory.ConfigureHttpClientCore(httpClient);
                configureClient(httpClient);
            });
}
