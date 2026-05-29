using Microsoft.EntityFrameworkCore;
using ZALUPA.Database.Classes;
using ZALUPA.Database.Configurations;

namespace ZALUPA.Database;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
 
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=zalupa;Username=postgres;Password=root");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfig()); // Configurations -> UserConfig
    }
}