namespace ManualApiClient.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

//ref: https://gist.github.com/davidfowl/c34633f1ddc519f030a1c0c5abe8e867
public static class HeaderPropagationExtensions
{
    public static IHttpClientBuilder AddHeaderPropagation(this IHttpClientBuilder builder, Action<HeaderPropagationOptions> configure)
    {
        builder.Services.Configure(configure);
        builder.AddHttpMessageHandler((sp) =>
        {
            var options = sp.GetRequiredService<IOptions<HeaderPropagationOptions>>();
            var contextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

            return new HeaderPropagationMessageHandler(
                options.Value,
                contextAccessor,
                sp.GetRequiredService<ILogger<HeaderPropagationMessageHandler>>());
        });

        return builder;
    }
}

public class HeaderPropagationOptions
{
    public IList<string> HeaderNames { get; set; } = new List<string>();
}
