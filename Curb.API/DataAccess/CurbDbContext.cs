using Curb.API.DataAccess.Configurations;
using Curb.API.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;

namespace Curb.API.DataAccess;

public class CurbDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
