using System.Diagnostics;

using Gridly.Domain;
using Gridly.Domain.Entities;
using Gridly.Infrastructure.DbContext;

using Microsoft.EntityFrameworkCore;

namespace MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(
        CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity(
            "Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LootDbContext>();
            
            await RunMigrationAsync(dbContext, cancellationToken);
            await SeedAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task RunMigrationAsync(
        LootDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private static async Task SeedAsync(LootDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
            {
                var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

                await dbContext.Roles.AddAsync(
                    new AppRole()
                    {
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        Name = UserRoles.Admin,
                        NormalizedName = UserRoles.Admin.ToUpper(),
                    }, cancellationToken);

                await dbContext.Roles.AddAsync(
                    new AppRole()
                    {
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        Name = UserRoles.Admin,
                        NormalizedName = UserRoles.Admin.ToUpper(),
                    }, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
        );
    }
}