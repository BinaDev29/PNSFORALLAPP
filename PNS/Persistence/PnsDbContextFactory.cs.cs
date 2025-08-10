// File Path: Persistence/PnsDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Persistence
{
    public class PnsDbContextFactory : IDesignTimeDbContextFactory<PnsDbContext>
    {
        public PnsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<PnsDbContext>();
            var connectionString = configuration.GetConnectionString("PnsConnectionString");

            builder.UseSqlServer(connectionString);

            return new PnsDbContext(builder.Options);
        }
    }
}