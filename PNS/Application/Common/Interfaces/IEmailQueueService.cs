// File Path: Application/Common/Interfaces/IEmailQueueService.cs
using Application.Models.Email;
using System;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IEmailQueueService
    {
        Task QueueEmailAsync(EnhancedEmailMessage emailMessage, int priority = 0);
        Task QueueBulkEmailAsync(BulkEmailMessage bulkEmailMessage, int priority = 0);
        Task<EmailQueueStatus> GetEmailStatusAsync(Guid emailId);
        Task RetryFailedEmailAsync(Guid emailId);
    }

    public enum EmailQueueStatus
    {
        Queued,
        Processing,
        Sent,
        Failed,
        Cancelled
    }
}