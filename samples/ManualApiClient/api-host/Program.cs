using ManualApiClient;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddPokemonClient(httpClient =>
{
    httpClient.BaseAddress = new(configuration["DadJokesClient:host"]);
    httpClient.AddDadJokesHeaders(
        configuration["DadJokesClient:hostHeader"],
        configuration["DADJOKES_TOKEN"]);
});

var app = builder.Build();

app.MapGet("/joke", async (IDadJokesApiClient client) => await client.GetRandomJokeAsync());

app.Run();
