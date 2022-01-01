namespace CrossCuttingPolly;

using Polly.Contrib.Simmy;

public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddRandomLatencyIssues(
        this IHttpClientBuilder builder,
        TimeSpan latency,
        double injectionRate,
        IConfiguration configuration)
    {
        var latencyPolicy = MonkeyPolicy.InjectLatencyAsync<HttpResponseMessage>(
            latency, injectionRate, enabled: () => configuration.GetSection("chaos").Get<bool>());

        return builder.AddPolicyHandler(latencyPolicy);
    }
}
