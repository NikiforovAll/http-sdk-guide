namespace ManualApiClient.Extensions;

/// <summary>
/// Changes default behavior of HttpClient in case of timeout exception.
/// Unwraps inner exception of <see cref="TaskCanceledException"/>
/// https://github.com/dotnet/runtime/issues/21965
/// https://devblogs.microsoft.com/dotnet/net-5-new-networking-improvements/#better-error-handling
/// </summary>
public class TimeoutThrowingDelegatingHandler : DelegatingHandler
{
    public TimeoutThrowingDelegatingHandler() { }

    public TimeoutThrowingDelegatingHandler(
        HttpMessageHandler innerHandler) : base(innerHandler) { }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        // TODO: this doesn't really
        // innerException is populated by HttpClient after DelegatingHandlers processing
        catch (TaskCanceledException timeoutException)
            when (timeoutException.InnerException is TimeoutException)
        {
            throw timeoutException.InnerException;
        }
    }
}
