namespace ManualApiClient;

public static class DadJokesApiClientFactory
{
    public static IDadJokesApiClient Create(
        string hostHeader,
        string apiKey,
        string? host = default,
        HttpClient? httpClient = default)
    {
        httpClient ??= new HttpClient();

        if (host is not null)
        {
            httpClient.BaseAddress = new Uri(host);
        }

        ConfigureHttpClient(httpClient, hostHeader, apiKey);

        return new DadJokesApiClient(httpClient);
    }

    internal static void ConfigureHttpClientCore(HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Accept.Add(new("application/json"));
    }

    internal static void ConfigureHttpClient(
        HttpClient httpClient,
        string host,
        string apiKey)
    {
        ConfigureHttpClientCore(httpClient);
        httpClient.AddDadJokesHeaders(host, apiKey);
    }
}
