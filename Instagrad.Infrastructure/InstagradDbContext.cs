using System.Reflection;
using Instagrad.Domain;
using Instagrad.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Instagrad.Infrastructure;

public class InstagradDbContext : DbContext
{
    private string _connectionString = "Server=localhost;Port=5432;Database=instagrad;User ID=postgres;Password=pass1234;";

    internal DbSet<User> Users => Set<User>();
    internal DbSet<Image> Images => Set<Image>();

    public InstagradDbContext()
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable(@"DB_CONNECTION_STRING"));
        optionsBuilder.UseNpgsql(_connectionString);
    }
}