using BattleGame.TestHelpers.TestHelpers;
using BattleGame.TestHelpers.TestHelpers.Dtos;
using FluentAssertions;
using Projects;
using System.Net.Http.Json;

namespace BattleGame.IntergrationTests
{
    public class IntergrationTest1
    {
        [Fact]
        public async Task TestGetLeaderboardThroughApiGateway()
        {
            var appHost = await DistributedApplicationTestingBuilder.CreateAsync<BattleGamePlatform_TestHost>();

            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            var _app = await appHost.BuildAsync();

            await _app.StartAsync();

            var client = _app.CreateHttpClient("gateway");

            var response = await client.GetAsync("/api/v1/leaderboards");
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<ApiResponse<List<LeaderboardGameDto>>>();

            results.Should().NotBeNull();
            results.IsSuccess.Should().BeFalse();
            results.Data.Should().BeNull();

            await _app.StopAsync();
        }
    }
}
