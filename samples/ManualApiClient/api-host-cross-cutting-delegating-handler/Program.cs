using ManualApiClient;
using ManualApiClient.Extensions;
using ManualApiClient.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((ctx, cfg) => cfg.WriteTo.Console());
var services = builder.Services;
services.AddHttpContextAccessor();
var configuration = builder.Configuration;

services.AddDadJokesApiClient(httpClient =>
{
    var host = configuration["DadJokesClient:host"];
    httpClient.BaseAddress = new(host);
    httpClient.Timeout = TimeSpan.FromSeconds(5);
    httpClient.AddDadJokesHeaders(host, configuration["DADJOKES_TOKEN"]);
}).AddHeaderPropagation(o => o.HeaderNames.Add("X-Correlation-ID"));

var app = builder.Build();

app.MapGet("/", async Task<Joke> (IDadJokesApiClient client) =>
    await client.GetRandomJokeAsync());

app.Run();
