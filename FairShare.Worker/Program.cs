using FairShare.API.Configurations;
using FairShare.Worker.Jobs;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddCustomDatabaseConfiguration(builder.Configuration);

builder.Services.AddHostedService<RefreshTokensCleanupJob>();

var host = builder.Build();
host.Run();
