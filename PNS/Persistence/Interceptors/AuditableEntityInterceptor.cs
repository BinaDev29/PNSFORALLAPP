// File Path: Persistence/Interceptors/AuditableEntityInterceptor.cs
using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Persistence.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public AuditableEntityInterceptor(ICurrentUserService currentUserService, IDateTime dateTime)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            var userId = _currentUserService.GetUserIdOrDefault();
            var now = _dateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<BaseDomainEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.CreatedDate = now;
                        entry.Entity.LastModifiedBy = userId;
                        entry.Entity.LastModifiedDate = now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = userId;
                        entry.Entity.LastModifiedDate = now;
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity.GetType().GetProperty("IsDeleted") != null)
                        {
                            // Soft delete
                            entry.State = EntityState.Modified;
                            entry.Entity.IsDeleted = true;
                            entry.Entity.DeletedBy = userId;
                            entry.Entity.DeletedDate = now;
                            entry.Entity.LastModifiedBy = userId;
                            entry.Entity.LastModifiedDate = now;
                        }
                        break;
                }
            }
        }
    }
}