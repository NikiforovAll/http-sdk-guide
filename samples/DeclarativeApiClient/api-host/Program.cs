using DeclarativeApiClient;
using DeclarativeApiClient.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((ctx, cfg) => cfg.WriteTo.Console());
var services = builder.Services;

var configuration = builder.Configuration;

services.AddDadJokesApiClient(httpClient =>
{
    var host = configuration["DadJokesClient:host"];
    httpClient.BaseAddress = new(host);
    httpClient.AddDadJokesHeaders(host, configuration["DADJOKES_TOKEN"]);
});

var app = builder.Build();

app.MapGet("/", async Task<Joke> (IDadJokesApiClient client) =>
{
    var jokeResponse = await client.GetRandomJokeAsync();

    return jokeResponse.Body.First();
});

app.Run();
