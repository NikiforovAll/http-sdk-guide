namespace ManualApiClient.Models;

public class JokeSearchResponse
{
    public bool Success { get; init; }

    public List<Joke> Body { get; init; } = new();
}
