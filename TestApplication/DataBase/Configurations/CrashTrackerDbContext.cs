using Microsoft.EntityFrameworkCore;
using TestApplication.DataBase.Entities;

namespace TestApplication.DataBase.Configurations
{
    public class CrashTrackerDbContext : DbContext
    {
        public CrashTrackerDbContext(DbContextOptions<CrashTrackerDbContext> options) : base(options) { }

        public DbSet<CrashEntity> Crash { get; set; }
        public DbSet<OperationEntity> Operation { get; set; }
        public DbSet<StatusEntity> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OperationCofiguration());
            modelBuilder.ApplyConfiguration(new CrashConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());

        }

        public void SeedStatuses()
        {
            if (!Status.Any())
            {
                Status.AddRange(
                    new StatusEntity { Id = Guid.NewGuid(), Name = "Новое" },
                    new StatusEntity { Id = Guid.NewGuid(), Name = "В работе" },
                    new StatusEntity { Id = Guid.NewGuid(), Name = "Выполнено" }
                );

                SaveChanges();
            }
        }

    }
}
