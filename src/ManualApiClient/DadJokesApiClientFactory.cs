namespace ManualApiClient;

public static class DadJokesApiClientFactory
{
    public static IDadJokesApiClient Create(
        HttpClient httpClient,
        string host,
        string apiKey)
    {
        httpClient.BaseAddress = new Uri(host);

        ConfigureHttpClient(httpClient, host, apiKey);

        return new DadJokesApiClient(httpClient);
    }

    public static IDadJokesApiClient Create(
        string host, string apiKey, params DelegatingHandler[] handlers)
    {
        var httpClient = new HttpClient();

        if (handlers.Length > 0)
        {
            _ = handlers.Aggregate((a, b) =>
            {
                a.InnerHandler = b;
                return b;
            });
            httpClient = new(handlers[0]);
        }
        httpClient.BaseAddress = new Uri(host);

        ConfigureHttpClient(httpClient, host, apiKey);

        return new DadJokesApiClient(httpClient);
    }

    internal static void ConfigureHttpClientCore(HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Accept.Clear();
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
