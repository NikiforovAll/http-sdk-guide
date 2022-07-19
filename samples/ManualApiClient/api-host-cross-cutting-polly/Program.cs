using CrossCuttingPolly;
using ManualApiClient;
using ManualApiClient.Extensions;
using Polly;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((ctx, cfg) => cfg.WriteTo.Console());
var services = builder.Services;
var configuration = builder.Configuration;

services.AddTransient<TimeoutThrowingDelegatingHandler>();
services.AddDadJokesApiClient(httpClient =>
{
    var host = configuration["DadJokesClient:host"];
    httpClient.BaseAddress = new(host);
    httpClient.Timeout = TimeSpan.FromSeconds(1);
    httpClient.AddDadJokesHeaders(host, configuration["DADJOKES_TOKEN"]);
})
    .AddRandomLatencyIssues(TimeSpan.FromSeconds(3), injectionRate: 0.8, configuration)
    .AddHttpMessageHandler<TimeoutThrowingDelegatingHandler>()
    .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
    {
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(10)
    }));

var app = builder.Build();

app.MapGet("/", async Task<IResult> (IDadJokesApiClient client, ILoggerFactory loggerFactory) =>
{
    try
    {
        return Results.Ok(await client.GetRandomJokeAsync());
    }
    catch (TimeoutException exception)
    {
        var logger = loggerFactory.CreateLogger("GetRandomJoke");
        logger.LogWarning(exception, "Service timeout...");
        return Results.BadRequest();
    }
});

app.Run();
