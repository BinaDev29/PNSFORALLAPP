// File Path: Persistence/PnsDbContext.cs
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class PnsDbContext : IdentityDbContext<AppUser>
    {
        public PnsDbContext(DbContextOptions<PnsDbContext> options) : base(options) { }

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
            base.OnModelCreating(modelBuilder);

            // ይህ መስመር ሁሉንም የIEntityTypeConfiguration ፋይሎች ያዋህዳል
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // ቀድሞ የነበሩት ግንኙነቶች
            modelBuilder.Entity<ApplicationNotificationTypeMap>(entity =>
            {
                entity.HasKey(map => new { map.ClientApplicationId, map.NotificationTypeId });
            });

            modelBuilder.Entity<NotificationHistory>(entity =>
            {
                entity.HasOne(h => h.Notification)
                    .WithMany(n => n.NotificationHistories) // ማስተካከያ: ከዚህ በፊት በDomain ላይ እንደጨመርነው
                    .HasForeignKey(h => h.NotificationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ለስላሳ ስረዛ (soft delete) የሚሆን Global Query Filter
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


        }

        private static void SetGlobalQueryFilter<T>(ModelBuilder builder) where T : BaseDomainEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}