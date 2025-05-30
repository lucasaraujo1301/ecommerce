using ECommerce.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECommerce.AuthService.Infrastructure.Data
{
    public class ECommerceAuthServiceDbContext(DbContextOptions<ECommerceAuthServiceDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public class ECommerceAuthServiceDbContextFactory : IDesignTimeDbContextFactory<ECommerceAuthServiceDbContext>
        {
            public ECommerceAuthServiceDbContext CreateDbContext(string[] args)
            {
                string jsonFile = "appsettings.json";

                string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                if (!string.IsNullOrEmpty(environment) && environment != "Production") {
                    jsonFile = $"appsettings.{environment}.json";
                }

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(jsonFile) // Adjust the path if needed
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                var optionsBuilder = new DbContextOptionsBuilder<ECommerceAuthServiceDbContext>();
                optionsBuilder.UseSqlite(connectionString);

                return new ECommerceAuthServiceDbContext(optionsBuilder.Options);
            }
        }
    }
    
}