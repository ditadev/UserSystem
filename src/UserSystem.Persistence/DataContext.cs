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
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations.User());
        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations.Role());
    }
}