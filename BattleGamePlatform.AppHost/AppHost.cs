var builder = DistributedApplication.CreateBuilder(args);

builder.AddApplicationServices();

builder.AddProject<Projects.BattleGame_TournamentHistoryService>("battlegame-tournamenthistoryservice");

builder.Build().Run();