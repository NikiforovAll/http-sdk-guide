namespace DeclarativeApiClient;

using DeclarativeApiClient.Models;
using Refit;

public interface IDadJokesApiClient
{
    /// <summary>
    /// Searches jokes by term.
    /// </summary>
    /// <param name="term"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Get("/joke/search")]
    Task<JokeSearchResponse> SearchAsync(
        string term,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a joke by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Get("/joke/{id}")]
    Task<Joke> GetJokeByIdAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a random joke.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Get("/random/joke")]
    Task<JokeSearchResponse> GetRandomJokeAsync(CancellationToken cancellationToken = default);
}
