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

        // Repositories
        private readonly IClientApplicationRepository _clientApplications;
        private readonly INotificationRepository _notifications;
        private readonly INotificationHistoryRepository _notificationHistories;
        private readonly INotificationTypeRepository _notificationTypes;
        private readonly IApplicationNotificationTypeMapRepository _applicationNotificationTypeMaps;
        private readonly IEmailTemplateRepository _emailTemplates;
        private readonly IPriorityRepository _priorities;
        private readonly IGenericRepository<NotificationHistory> _notificationHistory;

        // FIX: Add a private field for the SMS template repository
        private readonly ISmsTemplateRepository _smsTemplates;

        public UnitOfWork(PnsDbContext dbContext,
            IClientApplicationRepository clientApplications,
            INotificationRepository notifications,
            INotificationHistoryRepository notificationHistories,
            INotificationTypeRepository notificationTypes,
            IApplicationNotificationTypeMapRepository applicationNotificationTypeMaps,
            IEmailTemplateRepository emailTemplates,
            IPriorityRepository priorities,
            IGenericRepository<NotificationHistory> notificationHistory,
            // FIX: Add the SMS template repository to the constructor parameters
            ISmsTemplateRepository smsTemplates)
        {
            _dbContext = dbContext;
            _clientApplications = clientApplications;
            _notifications = notifications;
            _notificationHistories = notificationHistories;
            _notificationTypes = notificationTypes;
            _applicationNotificationTypeMaps = applicationNotificationTypeMaps;
            _emailTemplates = emailTemplates;
            _priorities = priorities;
            _notificationHistory = notificationHistory;
            // FIX: Initialize the new repository field
            _smsTemplates = smsTemplates;
        }

        // Public properties to access the repositories
        public IClientApplicationRepository ClientApplications => _clientApplications;
        public INotificationRepository Notifications => _notifications;
        public INotificationHistoryRepository NotificationHistories => _notificationHistories;
        public INotificationTypeRepository NotificationTypes => _notificationTypes;
        public IApplicationNotificationTypeMapRepository ApplicationNotificationTypeMaps => _applicationNotificationTypeMaps;
        public IEmailTemplateRepository EmailTemplates => _emailTemplates;
        public IPriorityRepository Priorities => _priorities;
        public IGenericRepository<NotificationHistory> NotificationHistory => _notificationHistory;

        // FIX: Add the public property for the SMS template repository
        public ISmsTemplateRepository SmsTemplates => _smsTemplates;

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