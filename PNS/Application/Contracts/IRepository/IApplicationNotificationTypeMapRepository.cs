// File Path: Application/Contracts/IRepository/IApplicationNotificationTypeMapRepository.cs
using Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface IApplicationNotificationTypeMapRepository : IGenericRepository<ApplicationNotificationTypeMap>
    {
        Task<ApplicationNotificationTypeMap?> Get(Guid clientApplicationId, Guid notificationTypeId, CancellationToken cancellationToken = default);
        Task Delete(Guid clientApplicationId, Guid notificationTypeId, CancellationToken cancellationToken = default);
    }
}