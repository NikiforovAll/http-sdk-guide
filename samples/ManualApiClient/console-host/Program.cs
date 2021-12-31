using ManualApiClient;

var host = "https://dad-jokes.p.rapidapi.com";
var apiKey = Environment.GetEnvironmentVariable("DADJOKES_TOKEN")
    ?? throw new InvalidOperationException("Unable to retrieve token.");

var client = DadJokesApiClientFactory.Create(host, apiKey);
var joke = await client.GetRandomJokeAsync();

Console.WriteLine($"{joke.Setup} {joke.Punchline}");
