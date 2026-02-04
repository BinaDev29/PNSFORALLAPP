// File Path: Application/Contracts/IRepository/INotificationRepository.cs
using Domain.Models;
using System;

namespace Application.Contracts.IRepository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<Application.DTO.Notification.NotificationStatisticsDto> GetStatisticsAsync(DateTime? startDate, DateTime? endDate, Guid? clientApplicationId, string? userId = null, bool isAdmin = false, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Notification>> GetByUserId(string? userId, bool isAdmin, CancellationToken cancellationToken = default);
    }
}