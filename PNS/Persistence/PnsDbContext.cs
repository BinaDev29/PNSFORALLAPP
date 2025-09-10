// File Path: Persistence/PnsDbContext.cs
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking; // Added using
using System.Text.Json; // Added using
using System.Collections.Generic; // Added using

namespace Persistence
{
    public class PnsDbContext : DbContext
    {
        public PnsDbContext(DbContextOptions<PnsDbContext> options) : base(options)
        {
        }

        public DbSet<ClientApplication> ClientApplications { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<ApplicationNotificationTypeMap> ApplicationNotificationTypeMaps { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<NotificationHistory> NotificationHistories { get; set; }
        public DbSet<SmsTemplate> SmsTemplates { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Apply existing configurations
            modelBuilder.Entity<ApplicationNotificationTypeMap>(entity =>
            {
                entity.HasKey(map => new { map.ClientApplicationId, map.NotificationTypeId });
            });

            modelBuilder.Entity<NotificationHistory>(entity =>
            {
                entity.HasOne(h => h.Notification)
                    .WithMany()
                    .HasForeignKey(h => h.NotificationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // FIX: Explicitly configure relationships to prevent shadow properties
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.NotificationType)
                .WithMany()
                .HasForeignKey(n => n.NotificationTypeId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Priority)
                .WithMany()
                .HasForeignKey(n => n.PriorityId);

            // FIX: Configure value comparers for collection properties
            modelBuilder.Entity<Notification>()
                .Property(n => n.To)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<object>>(v, (JsonSerializerOptions)null),
                    new ValueComparer<List<object>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()));

            modelBuilder.Entity<Notification>()
                .Property(n => n.Metadata)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null),
                    new ValueComparer<Dictionary<string, string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToDictionary(e => e.Key, e => e.Value)));

            // Global query filters for soft delete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseDomainEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(PnsDbContext)
                        .GetMethod(nameof(SetGlobalQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)?
                        .MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        private static void SetGlobalQueryFilter<T>(ModelBuilder builder) where T : BaseDomainEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Removed the SaveChanges method as it's not part of the DbContext's main purpose
    }
}