using ManualApiClient;
using ManualApiClient.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SpectreConsole;

var host = "https://dad-jokes.p.rapidapi.com";
var apiKey = Environment.GetEnvironmentVariable("DADJOKES_TOKEN")
    ?? throw new InvalidOperationException("Unable to retrieve token.");


var logger = ConfigureLogger();
var services = new ServiceCollection();
services.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory());

services.AddDadJokesApiClient(httpClient =>
{
    httpClient.BaseAddress = new(host);
    httpClient.AddDadJokesHeaders(host, apiKey);
});

var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<IDadJokesApiClient>();

var joke = await client.GetRandomJokeAsync();

logger.Information($"{joke.Setup} {joke.Punchline}");

static Serilog.ILogger ConfigureLogger()
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.FromLogContext()
        .WriteTo.SpectreConsole(
            "[{Level:u4}] {SourceContext}[{EventId}] {Message:lj}{NewLine}{Exception}",
            minLevel: LogEventLevel.Information)
        .CreateLogger();

    return Log.Logger;
}
