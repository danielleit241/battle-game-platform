using BattleGame.MatchService.Dtos;
using BattleGame.Shared.Client;
using BattleGame.Shared.Common;
using System.Text.Json;

namespace BattleGame.LeaderboardService.Clients
{
    public class UserClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : Client(httpClient, httpContextAccessor)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            ForwardJwtBearer();
            var response = await _httpClient.GetAsync($"api/v1/users/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result is null || result.Data is null)
            {
                return null;
            }

            return result.Data;
        }
    }
}
