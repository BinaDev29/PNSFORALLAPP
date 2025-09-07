// File Path: Persistence/PnsDbContext.cs
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityConfigurations;
using Persistence.Interceptors;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class PnsDbContext : DbContext
    {
        private readonly ICurrentUserService? _currentUserService;
        private readonly IDateTime? _dateTime;
        private readonly IDomainEventService? _domainEventService;

        public PnsDbContext(DbContextOptions<PnsDbContext> options) : base(options) 
        {
            _currentUserService = null;
            _dateTime = null;
            _domainEventService = null;
        }

        public PnsDbContext(DbContextOptions<PnsDbContext> options, 
            ICurrentUserService currentUserService, 
            IDateTime dateTime,
            IDomainEventService domainEventService) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _domainEventService = domainEventService;
        }

        public DbSet<ClientApplication> ClientApplications { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<ApplicationNotificationTypeMap> ApplicationNotificationTypeMaps { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<NotificationHistory> NotificationHistories { get; set; }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_currentUserService != null && _dateTime != null)
            {
                optionsBuilder.AddInterceptors(new AuditableEntityInterceptor(_currentUserService, _dateTime));
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DispatchDomainEventsAsync();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            DispatchDomainEventsAsync().GetAwaiter().GetResult();
            return base.SaveChanges();
        }

        private async Task DispatchDomainEventsAsync()
        {
            if (_domainEventService == null) return;

            var entities = ChangeTracker
                .Entries<AggregateRoot>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = entities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            entities.ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await _domainEventService.PublishAsync(domainEvent);
            }
        }
    }
}