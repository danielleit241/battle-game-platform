namespace BattleGame.MatchService.Clients
{
    public class UserClient
    {

        private readonly HttpClient _httpClient;

        public UserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetUserNameAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"/api/v1/users/{userId}");
            response.EnsureSuccessStatusCode();
            var userName = await response.Content.ReadAsStringAsync();
            return userName;
        }
    }
}
