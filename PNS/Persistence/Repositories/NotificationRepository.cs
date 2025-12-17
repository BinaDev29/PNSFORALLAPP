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
        // የ GetWhere ዘዴን እንደገና ለመተግበር new የሚለውን ቃል ተጠቀም
        public new async Task<IReadOnlyList<Notification>> GetWhere(Expression<Func<Notification, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Notification>()
                                   .Where(predicate)
                                   .ToListAsync(cancellationToken);
        }

        public async Task<Application.DTO.Notification.NotificationStatisticsDto> GetStatisticsAsync(DateTime? startDate, DateTime? endDate, Guid? clientApplicationId, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Notifications.AsQueryable();

            // Filter by Date Range
            if (startDate.HasValue)
                query = query.Where(n => n.CreatedDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(n => n.CreatedDate <= endDate.Value);

            // Filter by Client Application
            if (clientApplicationId.HasValue)
                query = query.Where(n => n.ClientApplicationId == clientApplicationId.Value);

            // Group by Status and Count
            var stats = await query.GroupBy(n => n.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            var result = new Application.DTO.Notification.NotificationStatisticsDto();

            foreach (var stat in stats)
            {
                result.TotalRequests += stat.Count;
                switch (stat.Status)
                {
                    case Domain.Enums.NotificationStatus.Pending:
                        result.Pending += stat.Count;
                        break;
                    case Domain.Enums.NotificationStatus.Sent:
                        result.Sent += stat.Count;
                        break;
                    case Domain.Enums.NotificationStatus.Failed:
                        result.Failed += stat.Count;
                        break;
                    case Domain.Enums.NotificationStatus.Seen:
                        result.Seen += stat.Count;
                        // Seen also counts as Sent/Success usually, but here it's a separate state in Enum.
                        // Ideally "Seen" implies "Sent" successfully.
                        break;
                    case Domain.Enums.NotificationStatus.Scheduled:
                        result.Scheduled += stat.Count;
                        break;
                }
            }

            // Calculate Success Rate (Sent + Seen) / Total * 100
            // Or (Total - Failed) / Total ?
            // Let's go with (Sent + Seen) / Total for now.
            if (result.TotalRequests > 0)
            {
                // Including 'Seen' as 'Sent' for success rate calc
                var successful = result.Sent + result.Seen;
                result.SuccessRate = Math.Round((double)successful / result.TotalRequests * 100, 2);
            }

            return result;
        }
    }
}