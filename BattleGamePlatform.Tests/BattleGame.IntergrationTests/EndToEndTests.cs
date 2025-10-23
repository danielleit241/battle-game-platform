using Aspire.Hosting;
using BattleGame.TestHelpers.TestHelpers;
using BattleGame.TestHelpers.TestHelpers.Dtos;
using Projects;
using System.Net.Http.Json;

namespace BattleGame.IntergrationTests
{
    public class EndToEndTests : IAsyncLifetime
    {
        private DistributedApplication _app = default!;
        private HttpClient _client = default!;
        private ResourceNotificationService _resourceWatcher = default!;

        public async Task InitializeAsync()
        {
            var appHost = await DistributedApplicationTestingBuilder.CreateAsync<BattleGame_TestHost>();

            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            _app = await appHost.BuildAsync();
            _resourceWatcher = _app.Services.GetRequiredService<ResourceNotificationService>();

            await _app.StartAsync();

            await _resourceWatcher
                .WaitForResourceAsync("gateway", KnownResourceStates.Running)
                .WaitAsync(TimeSpan.FromSeconds(60));

            _client = _app.CreateHttpClient("gateway");
        }

        public async Task DisposeAsync()
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }

        [Fact]
        public async Task EndToEnd_Workflow_Works()
        {
            var roleRegister = await _client.PostAsJsonAsync("/api/v1/users/roles", new
            {
                Name = "PLAYER"
            });
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
            Assert.Equal("Test Game", game.Name);

            var gameCompleted = await _client.PostAsJsonAsync($"/api/v1/games/{game.Id}/completed", new { });
            Assert.True(gameCompleted.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, gameCompleted.StatusCode);
            var gameCompletedResponse = await gameCompleted.Content.ReadFromJsonAsync<ApiResponse<GameDto>>();
            Assert.NotNull(gameCompletedResponse);
            Assert.True(gameCompletedResponse.IsSuccess);
            await Task.Delay(1000); //wait 1s ensure CompleteGameEvent processed by LeaderboardService
            var completedGame = gameCompletedResponse.Data;
            Assert.NotNull(completedGame);

            var leaderboardResponse = await _client.GetAsync($"/api/v1/leaderboards");
            Assert.Equal(HttpStatusCode.OK, leaderboardResponse.StatusCode);
            var leaderboard = await leaderboardResponse.Content.ReadFromJsonAsync<ApiResponse<List<LeaderboardDto>>>();
            Assert.NotNull(leaderboard);
            Assert.True(leaderboard.IsSuccess);
            Assert.NotNull(leaderboard.Data);
            Assert.True(leaderboard.Data.Count > 0);

            var leaderboardByGameResponse = await _client.GetAsync($"/api/v1/leaderboards/games/{game.Id}");
            Assert.Equal(HttpStatusCode.OK, leaderboardByGameResponse.StatusCode);
            var leaderboardByGame = await leaderboardByGameResponse.Content.ReadFromJsonAsync<ApiResponse<LeaderboardDto>>();
            Assert.NotNull(leaderboardByGame);
            Assert.True(leaderboardByGame.IsSuccess);
            Assert.NotNull(leaderboardByGame.Data);
        }
    }
}
