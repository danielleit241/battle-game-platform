using BattleGame.LeaderboardService.Dtos;
using BattleGame.Shared.Common;
using BattleGame.UserService.Common.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

namespace BattleGamePlatform.IntergrationTests
{
    public class EndToEndTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public EndToEndTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost:8080")
            });
        }

        [Fact]
        public async Task EndToEnd_Workflow_Works()
        {
            var roleRegister = await _client.PostAsJsonAsync("/api/v1/users/roles", new
            {
                roleName = "PLAYER"
            });
            Assert.True(roleRegister.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, roleRegister.StatusCode);
            var roleResponse = await roleRegister.Content.ReadFromJsonAsync<ApiResponse<RoleDto>>();
            Assert.NotNull(roleResponse);
            Assert.True(roleResponse.IsSuccess);
            var role = roleResponse.Data;
            Assert.NotNull(role);
            Assert.Equal("PLAYER", role.Name);

            var userRegister = await _client.PostAsJsonAsync("/api/v1/users/register", new
            {
                username = "testuser",
                email = "test@gmail.com",
                password = "123",
                roleId = role.Id
            });
            Assert.True(userRegister.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, userRegister.StatusCode);
            var userRegisterResponse = await userRegister.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
            Assert.NotNull(userRegisterResponse);
            Assert.True(userRegisterResponse.IsSuccess);
            await Task.Delay(1000); //wait 1s ensure CreateUserEvent processed by LeaderboardService
            var user = userRegisterResponse.Data;
            Assert.NotNull(user);
            Assert.Equal("testuser", user.Username);

            var userLogin = await _client.PostAsJsonAsync("/api/v1/users/login", new
            {
                username = "testuser",
                password = "123"
            });
            Assert.True(userLogin.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, userLogin.StatusCode);
            var loginResponse = await userLogin.Content.ReadFromJsonAsync<ApiResponse<TokenDto>>();
            Assert.NotNull(loginResponse);
            Assert.True(loginResponse.IsSuccess);
            var token = loginResponse.Data;
            Assert.NotNull(token);
            Assert.False(string.IsNullOrEmpty(token.Token));

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
            var gameCreate = await _client.PostAsJsonAsync("/api/v1/games", new
            {
                name = "Test Game",
                description = "This is a test game",
                maxPlayers = 4
            });
            Assert.True(gameCreate.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, gameCreate.StatusCode);
            var gameReponse = await gameCreate.Content.ReadFromJsonAsync<ApiResponse<GameDto>>();
            Assert.NotNull(gameReponse);
            Assert.True(gameReponse.IsSuccess);
            await Task.Delay(1000); //wait 1s ensure CreateGameEvent processed by LeaderboardService
            var game = gameReponse.Data;
            Assert.NotNull(game);
            Assert.Equal("Test Game", game.GameName);

            var gameCompleted = await _client.PostAsJsonAsync($"/api/v1/games/{game.GameId}/completed", new { });
            Assert.True(gameCompleted.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, gameCompleted.StatusCode);
            var gameCompletedResponse = await gameCompleted.Content.ReadFromJsonAsync<ApiResponse<GameDto>>();
            Assert.NotNull(gameCompletedResponse);
            Assert.True(gameCompletedResponse.IsSuccess);
            await Task.Delay(1000); //wait 1s ensure CompleteGameEvent processed by LeaderboardService
            var completedGame = gameCompletedResponse.Data;
            Assert.NotNull(completedGame);

            var leaderboardResponse = await _client.GetAsync($"/api/v1/leaderboards");
            Assert.True(leaderboardResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, leaderboardResponse.StatusCode);
            var leaderboard = await leaderboardResponse.Content.ReadFromJsonAsync<ApiResponse<List<LeaderboardDto>>>();
            Assert.NotNull(leaderboard);
            Assert.True(leaderboard.IsSuccess);
            Assert.NotNull(leaderboard.Data);
            Assert.True(leaderboard.Data.Count > 0);
            var topPlayer = leaderboard.Data.FirstOrDefault();
            Assert.NotNull(topPlayer);
            Assert.Equal(user.Id, topPlayer.UserId);

            var leaderboardByGameResponse = await _client.GetAsync($"/api/v1/leaderboards/games/{game.GameId}");
            Assert.True(leaderboardByGameResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, leaderboardByGameResponse.StatusCode);
            var leaderboardByGame = await leaderboardByGameResponse.Content.ReadFromJsonAsync<ApiResponse<List<LeaderboardDto>>>();
            Assert.NotNull(leaderboardByGame);
            Assert.True(leaderboardByGame.IsSuccess);
            Assert.NotNull(leaderboardByGame.Data);
            Assert.True(leaderboardByGame.Data.Count > 0);
            var topPlayerByGame = leaderboardByGame.Data.FirstOrDefault();
            Assert.NotNull(topPlayerByGame);
            Assert.Equal(user.Id, topPlayerByGame.UserId);
        }
    }
}
