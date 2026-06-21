using Gridly.Infrastructure.DbContext;

using MigrationService;

using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();
builder.AddNpgsqlDbContext<LootDbContext>("loot-db");
var host = builder.Build();
host.Run();