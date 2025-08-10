// File Path: Persistence/Repositories/UnitOfWork.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PnsDbContext _dbContext;

        // Repositoriesን ለማስተዳደር የሚረዱ properties
        public IClientApplicationRepository ClientApplications { get; }
        public INotificationRepository Notifications { get; }
        public INotificationHistoryRepository NotificationHistories { get; }
        public IGenericRepository<NotificationHistory> NotificationHistory { get; }
        public INotificationTypeRepository NotificationTypes { get; }
        public IApplicationNotificationTypeMapRepository ApplicationNotificationTypeMaps { get; }
        public IEmailTemplateRepository EmailTemplates { get; }
        public IPriorityRepository Priorities { get; }

        public UnitOfWork(PnsDbContext dbContext,
            IClientApplicationRepository clientApplicationRepository,
            INotificationRepository notificationRepository,
            INotificationHistoryRepository notificationHistoryRepository,
            INotificationTypeRepository notificationTypeRepository,
            IApplicationNotificationTypeMapRepository applicationNotificationTypeMapRepository,
            IEmailTemplateRepository emailTemplateRepository,
            IPriorityRepository priorityRepository,
            IGenericRepository<NotificationHistory> notificationHistoryGenericRepository) // ይህንን መስመር አክል
        {
            _dbContext = dbContext;
            ClientApplications = clientApplicationRepository;
            Notifications = notificationRepository;
            NotificationHistories = notificationHistoryRepository;
            NotificationTypes = notificationTypeRepository;
            ApplicationNotificationTypeMaps = applicationNotificationTypeMapRepository;
            EmailTemplates = emailTemplateRepository;
            Priorities = priorityRepository;
            NotificationHistory = notificationHistoryGenericRepository; // ይህንን መስመር አክል
        }

        public async Task<int> Save(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}