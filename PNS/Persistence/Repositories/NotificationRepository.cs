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
    }
}