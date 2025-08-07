using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
        public class ApplicationNotificationTypeMapRepository : GenericRepository<ApplicationNotificationTypeMap>, IApplicationNotificationTypeMapRepository
    {
        private readonly PnsDbContext _dbContext;

        public ApplicationNotificationTypeMapRepository(PnsDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // Composite keyን ተጠቅሞ አንድን ApplicationNotificationTypeMap ለማግኘት
        public async Task<ApplicationNotificationTypeMap> Get(Guid clientApplicationId, Guid notificationTypeId)
        {
            return await _dbContext.Set<ApplicationNotificationTypeMap>()
                .FirstOrDefaultAsync(m => m.ClientApplicationId == clientApplicationId && m.NotificationTypeId == notificationTypeId)
                ?? throw new InvalidOperationException("ApplicationNotificationTypeMap not found.");
        }

        // Composite keyን ተጠቅሞ አንድን ApplicationNotificationTypeMap ለመሰረዝ
        public async Task Delete(Guid clientApplicationId, Guid notificationTypeId)
        {
            var map = await Get(clientApplicationId, notificationTypeId);
            if (map != null)
            {
                _dbContext.Set<ApplicationNotificationTypeMap>().Remove(map);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}