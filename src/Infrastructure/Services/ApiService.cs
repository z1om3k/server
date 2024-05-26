using Core.Services;

namespace Infrastructure.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> FetchFromGitHubAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync($"https://api.github.com/{endpoint}");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> FetchFromSpotifyAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/{endpoint}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
