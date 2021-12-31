namespace ManualApiClient;

using ManualApiClient.Constants;

public static class HttpClientExtensions
{
    public static HttpClient AddDadJokesHeaders(
        this HttpClient httpClient,
        string host,
        string apiKey)
    {
        var headers = httpClient.DefaultRequestHeaders;
        headers.Add(ApiConstants.HostHeader, host);
        headers.Add(ApiConstants.ApiKeyHeader, apiKey);

        return httpClient;
    }
}
