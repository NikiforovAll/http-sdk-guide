namespace HeaderPropagation;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

public class HeaderPropagationMessageHandler : DelegatingHandler
{
    private readonly HeaderPropagationOptions options;
    private readonly IHttpContextAccessor contextAccessor;
    private readonly ILogger<HeaderPropagationMessageHandler>? logger;

    public HeaderPropagationMessageHandler(
        HeaderPropagationOptions options,
        IHttpContextAccessor contextAccessor,
        ILogger<HeaderPropagationMessageHandler>? logger = default)
    {
        this.options = options;
        this.contextAccessor = contextAccessor;
        this.logger = logger;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (this.contextAccessor.HttpContext != null)
        {
            this.logger?.LogInformation($"Entering {nameof(HeaderPropagationMessageHandler)}");

            foreach (var headerName in this.options.HeaderNames)
            {
                var headerValue = this.contextAccessor.HttpContext.Request.Headers[headerName];
                if (StringValues.IsNullOrEmpty(headerValue))
                {
                    continue;
                }
                this.logger?.LogDebug("Setting header {HeaderName}", headerName);
                request.Headers.TryAddWithoutValidation(headerName, (string[])headerValue);
            }
        }

        return base.SendAsync(request, cancellationToken);
    }
}
