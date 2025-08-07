// IApplicationNotificationTypeMapRepository.cs
using Domain.Models;
using System;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    // ይህ Repository ለዚህ ሞዴል ልዩ የሆኑ ሜተዶችን ይዟል
    public interface IApplicationNotificationTypeMapRepository : IGenericRepository<ApplicationNotificationTypeMap>
    {
        Task<ApplicationNotificationTypeMap> Get(Guid clientApplicationId, Guid notificationTypeId);

        Task Delete(Guid clientApplicationId, Guid notificationTypeId);
    }
}