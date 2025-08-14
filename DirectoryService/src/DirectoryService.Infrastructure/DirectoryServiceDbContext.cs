using DirectoryService.Domain.Entities.Departments;
using DirectoryService.Domain.Entities.Locations;
using DirectoryService.Domain.Entities.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure
{
    public class DirectoryServiceDbContext(string ConnectionString) : DbContext
    {
        public DbSet<Department> Departments => Set<Department>();

        public DbSet<Location> Department => Set<Location>();

        public DbSet<Position> Positions => Set<Position>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);

            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(DirectoryServiceDbContext).Assembly);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}