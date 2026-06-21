using Gridly.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Loot.Infrastructure.DbContext;

public class LootDbContext(DbContextOptions<LootDbContext> options) : IdentityDbContext<AppUser, AppRole, Guid>(options)
{
    public DbSet<Board> Boards { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<TaskItem>  Tasks { get; set; }
    public DbSet<BoardMember> Members { get; set; }
}

public class LootDbContextFactory : IDesignTimeDbContextFactory<LootDbContext>
{
    public LootDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LootDbContext>();
        
        optionsBuilder.UseNpgsql("Host=localhost;Port=50291;Username=postgres;Password=+UhrnR0N2.}(}Dz(y}4Zyz\n\n");

        return new LootDbContext(optionsBuilder.Options);
    }
}