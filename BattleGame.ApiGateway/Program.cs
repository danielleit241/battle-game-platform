var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var yarp = builder.Services.AddYarp()

var app = builder.Build();

app.Run();
