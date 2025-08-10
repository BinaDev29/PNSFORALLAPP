// File Path: Persistence/Repositories/NotificationRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;

namespace Persistence.Repositories
{
    public class NotificationRepository(PnsDbContext dbContext) : GenericRepository<Notification>(dbContext), INotificationRepository
    {
    }
}