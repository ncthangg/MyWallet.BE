using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MyWallet.Domain.Constants;

namespace MyWallet.Infrastructure.Persistence.MyDbContext
{
    public class MyWalletDbContextFactory : IDesignTimeDbContextFactory<MyWalletDbContext>
    {
        public MyWalletDbContext CreateDbContext(string[] args)
        {
            // Try to find the API project from different possible locations
            var currentDirectory = Directory.GetCurrentDirectory();
            var apiProjectPath = Path.Combine(currentDirectory, "../MyWallet.API");

            // Fallback: if running from solution root
            if (!Directory.Exists(apiProjectPath))
            {
                apiProjectPath = Path.Combine(currentDirectory, "MyWallet.API");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString(Database.DefaultConnection);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in appsettings.json. " +
                    $"Searched in: {apiProjectPath}");
            }

            var optionsBuilder = new DbContextOptionsBuilder<MyWalletDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MyWalletDbContext(optionsBuilder.Options);
        }
    }
}
