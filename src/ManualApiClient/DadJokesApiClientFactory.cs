namespace ManualApiClient;

public static class DadJokesApiClientFactory
{
    public static IDadJokesApiClient Create(
        HttpClient httpClient,
        string host,
        string apiKey)
    {
        httpClient ??= new HttpClient();

        httpClient.BaseAddress = new Uri(host);

        ConfigureHttpClient(httpClient, host, apiKey);

        return new DadJokesApiClient(httpClient);
    }

    public static IDadJokesApiClient Create(string host, string apiKey) =>
        Create(new HttpClient(), host, apiKey);

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
