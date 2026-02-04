// File Path: Persistence/Repositories/NotificationHistoryRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class NotificationHistoryRepository(PnsDbContext dbContext) : GenericRepository<NotificationHistory>(dbContext), INotificationHistoryRepository
    {
        public async Task<IReadOnlyList<NotificationHistory>> GetNotificationHistoriesWithDetails(string? userId = null, bool isAdmin = false, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.NotificationHistories.AsQueryable();

            if (!isAdmin && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(h => h.CreatedBy == userId);
            }

            return await query
                .AsNoTracking()
                .Include(h => h.Notification)
                    .ThenInclude(n => n!.NotificationType)
                .ToListAsync(cancellationToken);
        }

        public async Task<NotificationHistory?> GetNotificationHistoryWithDetails(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.NotificationHistories
                .AsNoTracking()
                .Include(h => h.Notification)
                    .ThenInclude(n => n!.NotificationType)
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }
    }
}