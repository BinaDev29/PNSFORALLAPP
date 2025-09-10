// File Path: Infrastructure/BackgroundServices/EmailQueueService.cs
using Application.Common.Interfaces;
using Application.Contracts;
using Application.Models.Email;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.BackgroundServices
{
    public class EmailQueueService : IEmailQueueService
    {
        private readonly EmailQueueProcessor _processor;
        private readonly ILogger<EmailQueueService> _logger;

        public EmailQueueService(EmailQueueProcessor processor, ILogger<EmailQueueService> logger)
        {
            _processor = processor;
            _logger = logger;
        }

        public Task QueueEmailAsync(EnhancedEmailMessage emailMessage, int priority = 0)
        {
            _processor.QueueEmail(emailMessage, priority);
            return Task.CompletedTask;
        }

        public Task QueueBulkEmailAsync(BulkEmailMessage bulkEmailMessage, int priority = 0)
        {
            foreach (var recipient in bulkEmailMessage.Recipients)
            {
                var enhancedEmail = new EnhancedEmailMessage
                {
                    Id = Guid.NewGuid(),
                    From = bulkEmailMessage.From,
                    To = new List<string> { recipient },
                    Subject = bulkEmailMessage.Subject,
                    BodyHtml = bulkEmailMessage.BodyHtml,
                    Priority = (EmailPriority)priority,
                    MaxRetries = bulkEmailMessage.MaxRetries
                };

                _processor.QueueEmail(enhancedEmail, priority);
            }
            return Task.CompletedTask;
        }

        public Task<EmailQueueStatus> GetEmailStatusAsync(Guid emailId)
        {
            return Task.FromResult(EmailQueueStatus.Queued);
        }

        public Task RetryFailedEmailAsync(Guid emailId)
        {
            _logger.LogInformation("Retrying failed email {EmailId}", emailId);
            return Task.CompletedTask;
        }
    }
}