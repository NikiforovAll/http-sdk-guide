using ManualApiClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SpectreConsole;

var host = "https://dad-jokes.p.rapidapi.com";
var hostHeader = "dad-jokes.p.rapidapi.com";
var apiKey = Environment.GetEnvironmentVariable("DADJOKES_TOKEN")
    ?? throw new InvalidOperationException("Unable to retrieve token.");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .Enrich.FromLogContext()
    .WriteTo.SpectreConsole(
        "[{Level:u4}] {SourceContext} {Message:lj}{NewLine}{Exception}",
        minLevel: LogEventLevel.Information)
    .CreateLogger();

var services = new ServiceCollection();
services.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory());

services.AddPokemonClient(httpClient =>
{
    httpClient.BaseAddress = new(host);
    httpClient.AddDadJokesHeaders(hostHeader, apiKey);
});

var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<IDadJokesApiClient>();

var joke = await client.GetRandomJokeAsync();

Log.Logger.Information($"{joke.Setup} {joke.Punchline}");
