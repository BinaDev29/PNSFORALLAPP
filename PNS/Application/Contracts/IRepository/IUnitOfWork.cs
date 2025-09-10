// File Path: Application/Contracts/IRepository/IUnitOfWork.cs
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IClientApplicationRepository ClientApplications { get; }
        INotificationRepository Notifications { get; }
        INotificationHistoryRepository NotificationHistories { get; }
        INotificationTypeRepository NotificationTypes { get; }
        IApplicationNotificationTypeMapRepository ApplicationNotificationTypeMaps { get; }
        IEmailTemplateRepository EmailTemplates { get; }
        IPriorityRepository Priorities { get; }
        ISmsTemplateRepository SmsTemplates { get; } // ይህ ጎድሎ ነበር፣ ጨምር።

        Task<int> Save(CancellationToken cancellationToken);
    }
}