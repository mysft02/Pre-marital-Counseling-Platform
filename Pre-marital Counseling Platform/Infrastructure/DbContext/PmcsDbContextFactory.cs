using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SWP391.Infrastructure.DbContext
{
    public class PmcsDbContextFactory
    {
        public PmcsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json")
              .Build();

            Console.WriteLine($"Using ConnectionString: {configuration.GetConnectionString("DatabaseConnection")}");

            var optionsBuilder = new DbContextOptionsBuilder<PmcsDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"));

            return new PmcsDbContext(optionsBuilder.Options);
        }
    }
}
