// File Path: Persistence/Repositories/NotificationTypeRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;

namespace Persistence.Repositories
{
    public class NotificationTypeRepository(PnsDbContext dbContext) : GenericRepository<NotificationType>(dbContext), INotificationTypeRepository
    {
    }
}