using BattleGame.MatchService.Dtos;
using BattleGame.Shared.Common;
using System.Text.Json;

namespace BattleGame.MatchService.Clients
{
    public class GameClient(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<IReadOnlyCollection<GameDto>> GetAllGamesAsync()
        {
            var response = await _httpClient.GetAsync("api/v1/games");

            if (!response.IsSuccessStatusCode)
            {
                return [];
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyCollection<GameDto>>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result is null || result.Data is null)
            {
                return [];
            }

            return result.Data;
        }

    }
}
