using BattleGame.TransactionOutbox.MessageConsuming;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<MessageConsumingWorker>();

var host = builder.Build();
host.Run();
