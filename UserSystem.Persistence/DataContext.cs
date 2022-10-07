using Microsoft.EntityFrameworkCore;
using UserSystem.Models;

namespace UserService.Persistence;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // either apply each config:
        // modelBuilder.ApplyConfiguration(new EntityTypeConfigurations.User());
        
        // ...or use one config to register all configs in assembly:
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(User).Assembly);
    }
}