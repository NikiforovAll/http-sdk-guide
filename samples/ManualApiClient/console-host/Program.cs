using ManualApiClient;

var host = "https://dad-jokes.p.rapidapi.com";
var hostHeader = "dad-jokes.p.rapidapi.com";
var apiKey = "";

var client = DadJokesApiClientFactory.Create(hostHeader, apiKey, host);
var joke = await client.GetRandomJokeAsync();

Console.WriteLine($"{joke.Setup} {joke.Punchline}");
