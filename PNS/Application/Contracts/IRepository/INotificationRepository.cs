// File Path: Application/Contracts/IRepository/INotificationRepository.cs
using Domain.Models;

namespace Application.Contracts.IRepository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<Application.DTO.Notification.NotificationStatisticsDto> GetStatisticsAsync(DateTime? startDate, DateTime? endDate, Guid? clientApplicationId, CancellationToken cancellationToken = default);
    }
}