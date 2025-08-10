// File Path: Persistence/Repositories/NotificationHistoryRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;

namespace Persistence.Repositories
{
    public class NotificationHistoryRepository(PnsDbContext dbContext) : GenericRepository<NotificationHistory>(dbContext), INotificationHistoryRepository
    {
    }
}