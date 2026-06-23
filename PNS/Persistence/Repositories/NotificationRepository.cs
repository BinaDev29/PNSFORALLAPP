// File Path: Persistence/Repositories/NotificationRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class NotificationRepository(PnsDbContext dbContext) : GenericRepository<Notification>(dbContext), INotificationRepository
    {
        public async Task<IReadOnlyList<Notification>> GetByUserId(string? userId, bool isAdmin, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Notifications.AsQueryable();

            if (!isAdmin && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(n => n.CreatedBy == userId);
            }

            return await query.ToListAsync(cancellationToken);
        }

        // የ GetWhere ዘዴን እንደገና ለመተግበር new የሚለውን ቃል ተጠቀም
        public new async Task<IReadOnlyList<Notification>> GetWhere(Expression<Func<Notification, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Notification>()
                                   .Where(predicate)
                                   .ToListAsync(cancellationToken);
        }

        public async Task<Application.DTO.Notification.NotificationStatisticsDto> GetStatisticsAsync(DateTime? startDate, DateTime? endDate, Guid? clientApplicationId, string? userId = null, bool isAdmin = false, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Notifications.AsQueryable();

            // Filter by Admin status
            if (!isAdmin && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(n => n.CreatedBy == userId);
            }

            // Filter by Date Range
            if (startDate.HasValue)
                query = query.Where(n => n.CreatedDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(n => n.CreatedDate <= endDate.Value);

            // Filter by Client Application
            if (clientApplicationId.HasValue)
                query = query.Where(n => n.ClientApplicationId == clientApplicationId.Value);

            var result = new Application.DTO.Notification.NotificationStatisticsDto();
            result.TotalRequests = await query.CountAsync(cancellationToken);

            // Get real status from NotificationHistories
            var historyQuery = _dbContext.NotificationHistories.AsQueryable();
            
            if (!isAdmin && !string.IsNullOrEmpty(userId))
            {
                historyQuery = historyQuery.Where(h => h.Notification.CreatedBy == userId);
            }
            if (startDate.HasValue)
                historyQuery = historyQuery.Where(h => h.SentDate >= startDate.Value);
            if (endDate.HasValue)
                historyQuery = historyQuery.Where(h => h.SentDate <= endDate.Value);
            if (clientApplicationId.HasValue)
                historyQuery = historyQuery.Where(h => h.Notification.ClientApplicationId == clientApplicationId.Value);

            var historyStats = await historyQuery.GroupBy(h => h.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            foreach (var stat in historyStats)
            {
                var statusStr = stat.Status?.ToUpper();
                if (statusStr == "SENT" || statusStr == "SUCCESS")
                    result.Sent += stat.Count;
                else if (statusStr == "FAILED" || statusStr == "ERROR")
                    result.Failed += stat.Count;
                else if (statusStr == "QUEUED" || statusStr == "PENDING")
                    result.Pending += stat.Count;
                else if (statusStr == "SEEN")
                    result.Seen += stat.Count;
            }

            // Calculate Success Rate based only on processed messages
            var processed = result.Sent + result.Seen + result.Failed;
            if (processed > 0)
            {
                var successful = result.Sent + result.Seen;
                result.SuccessRate = Math.Round((double)successful / processed * 100, 2);
            }
            else
            {
                result.SuccessRate = 0;
            }

            return result;
        }
    }
}