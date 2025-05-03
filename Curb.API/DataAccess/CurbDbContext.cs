using Curb.API.DataAccess.Configurations;
using Curb.API.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;

namespace Curb.API.DataAccess;

internal class CurbDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new SessionConfiguration());
    }
}
