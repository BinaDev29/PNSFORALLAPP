// File Path: Application/Contracts/IRepository/INotificationHistoryRepository.cs
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Contracts.IRepository
{
    public interface INotificationHistoryRepository : IGenericRepository<NotificationHistory>
    {
        Task<IReadOnlyList<NotificationHistory>> GetNotificationHistoriesWithDetails(string? userId = null, bool isAdmin = false, CancellationToken cancellationToken = default);
        Task<NotificationHistory?> GetNotificationHistoryWithDetails(Guid id, CancellationToken cancellationToken = default);
    }
}