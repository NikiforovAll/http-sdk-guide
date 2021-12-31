namespace ManualApiClient;

using ManualApiClient.Models;

public interface IDadJokesApiClient
{
    /// <summary>
    /// Searches jokes by term.
    /// </summary>
    /// <param name="term"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<JokeSearchResponse> SearchAsync(
        string term,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a joke by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Joke> GetJokeByIdAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a random joke.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Joke> GetRandomJokeAsync(CancellationToken cancellationToken = default);
}
