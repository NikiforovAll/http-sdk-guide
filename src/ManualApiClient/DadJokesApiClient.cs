namespace ManualApiClient;

using System.Net.Http.Json;
using System.Threading.Tasks;
using Flurl;
using ManualApiClient.Constants;
using ManualApiClient.Models;

/// <inheritdoc/>
public class DadJokesApiClient : IDadJokesApiClient
{
    private readonly HttpClient httpClient;

    public DadJokesApiClient(HttpClient httpClient) =>
        this.httpClient = httpClient
            ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<Joke> GetJokeByIdAsync(
        string id, CancellationToken cancellationToken = default)
    {
        var path = ApiUrlConstants.GetJokeById.AppendPathSegment(id);

        var joke = await this.httpClient
            .GetFromJsonAsync<Joke>(path, cancellationToken);

        return joke ?? new();
    }

    public async Task<Joke> GetRandomJokeAsync(CancellationToken cancellationToken = default)
    {
        var jokes = await this.httpClient.GetFromJsonAsync<JokeSearchResponse>(
            ApiUrlConstants.GetRandomJoke, cancellationToken);

        if (jokes is null or { Body.Count: 0 } or { Success: false })
        {
            throw new InvalidOperationException("This API is no joke.");
        }

        return jokes.Body.First();
    }

    public async Task<JokeSearchResponse> SearchAsync(
        string term, CancellationToken cancellationToken = default)
    {
        var jokes = await this.httpClient
            .GetFromJsonAsync<JokeSearchResponse>(
                ApiUrlConstants.JokeSearch, cancellationToken);

        return jokes ?? new();
    }
}
