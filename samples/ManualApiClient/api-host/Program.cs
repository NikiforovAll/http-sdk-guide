using ManualApiClient;
using ManualApiClient.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddDadJokesApiClient(httpClient =>
{
    var host = configuration["DadJokesClient:host"];
    httpClient.BaseAddress = new(host);
    httpClient.AddDadJokesHeaders(host, configuration["DADJOKES_TOKEN"]);
});

var app = builder.Build();

app.MapGet("/", async (IDadJokesApiClient client) => await client.GetRandomJokeAsync());

app.Run();
