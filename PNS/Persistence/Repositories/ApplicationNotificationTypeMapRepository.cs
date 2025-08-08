// File Path: Persistence/Repositories/ApplicationNotificationTypeMapRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories; // 🟢 ይህን መስመር መጨመር አስፈላጊ ነው

// ከ`GenericRepository` በትክክል መውረስ
public class ApplicationNotificationTypeMapRepository(PnsDbContext dbContext)
    : GenericRepository<ApplicationNotificationTypeMap>(dbContext), IApplicationNotificationTypeMapRepository
{
    public async Task<ApplicationNotificationTypeMap?> Get(Guid clientApplicationId, Guid notificationTypeId, CancellationToken cancellationToken = default)
    {
        // 🟢 የ_dbContextን ከ`base class` (GenericRepository) መጠቀም
        return await _dbContext.Set<ApplicationNotificationTypeMap>()
            .FirstOrDefaultAsync(m => m.ClientApplicationId == clientApplicationId && m.NotificationTypeId == notificationTypeId, cancellationToken);
    }

    public async Task Delete(Guid clientApplicationId, Guid notificationTypeId, CancellationToken cancellationToken = default)
    {
        var map = await Get(clientApplicationId, notificationTypeId, cancellationToken);
        if (map is not null)
        {
            await base.Delete(map, cancellationToken); // 🟢 `base` classን በመጠቀም Delete methodን መጥራት
        }
    }
}