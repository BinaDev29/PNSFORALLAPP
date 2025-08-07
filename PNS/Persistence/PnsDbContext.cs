// File Path: Persistence/PnsDbContext.cs
using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class PnsDbContext : DbContext
    {
        public PnsDbContext(DbContextOptions<PnsDbContext> options) : base(options) { }

        public DbSet<ClientApplication> ClientApplications { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<ApplicationNotificationTypeMap> ApplicationNotificationTypeMaps { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<NotificationHistory> NotificationHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseDomainEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}