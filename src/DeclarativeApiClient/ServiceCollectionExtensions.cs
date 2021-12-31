namespace DeclarativeApiClient;

using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Refit;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddPokemonClient(
        this IServiceCollection services,
        Action<HttpClient> configureClient)
    {
        var settings = new RefitSettings()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
            })
        };

        return services.AddRefitClient<IDadJokesApiClient>(settings)
            .ConfigureHttpClient((httpClient) =>
            {
                DadJokesApiClientFactory.ConfigureHttpClientCore(httpClient);
                configureClient(httpClient);
            });
    }
}
