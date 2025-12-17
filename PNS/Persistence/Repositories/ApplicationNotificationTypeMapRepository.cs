// File Path: Persistence/Repositories/ApplicationNotificationTypeMapRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ApplicationNotificationTypeMapRepository(PnsDbContext dbContext) : GenericRepository<ApplicationNotificationTypeMap>(dbContext), IApplicationNotificationTypeMapRepository
    {
        public async Task<ApplicationNotificationTypeMap?> GetByKeys(Guid clientApplicationId, Guid notificationTypeId, CancellationToken cancellationToken)
        {
            return await _dbContext.ApplicationNotificationTypeMaps
                .FirstOrDefaultAsync(q => q.ClientApplicationId == clientApplicationId && q.NotificationTypeId == notificationTypeId, cancellationToken);
        }
    }
}