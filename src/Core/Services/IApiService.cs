namespace Core.Services
{
    public interface IApiService
    {
        Task<string> FetchFromGitHubAsync(string endpoint);
        Task<string> FetchFromSpotifyAsync(string endpoint);
    }
}
