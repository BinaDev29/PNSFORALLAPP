// File Path: Application/EventHandlers/NotificationCreatedEventHandler.cs
using Application.Common.Interfaces;
using Application.Models.Email;
using Application.Services;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EventHandlers
{
    public class NotificationCreatedEventHandler : INotificationHandler<NotificationCreatedEvent>
    {
        private readonly IEmailQueueService _emailQueueService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ILogger<NotificationCreatedEventHandler> _logger;

        public NotificationCreatedEventHandler(
            IEmailQueueService emailQueueService,
            IEmailTemplateService emailTemplateService,
            ILogger<NotificationCreatedEventHandler> logger)
        {
            _emailQueueService = emailQueueService;
            _emailTemplateService = emailTemplateService;
            _logger = logger;
        }

        public async Task Handle(NotificationCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing NotificationCreatedEvent for notification {NotificationId}", 
                    notification.NotificationId);

                // Process the notification template
                var processedHtml = await _emailTemplateService.ProcessNotificationTemplateAsync(
                    notification.NotificationId);

                // Create enhanced email message
                var emailMessage = new EnhancedEmailMessage
                {
                    From = "noreply@yourapp.com", // This should come from configuration
                    To = notification.Recipients,
                    Subject = notification.Title,
                    BodyHtml = processedHtml,
                    TrackingId = notification.NotificationId.ToString(),
                    EnableTracking = true,
                    Metadata = new Dictionary<string, object>
                    {
                        ["NotificationId"] = notification.NotificationId,
                        ["ClientApplicationId"] = notification.ClientApplicationId,
                        ["NotificationTypeId"] = notification.NotificationTypeId,
                        ["PriorityId"] = notification.PriorityId
                    }
                };

                // Queue the email for sending
                await _emailQueueService.QueueEmailAsync(emailMessage, 
                    GetPriorityFromId(notification.PriorityId));

                _logger.LogInformation("Successfully queued email for notification {NotificationId}", 
                    notification.NotificationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing NotificationCreatedEvent for notification {NotificationId}", 
                    notification.NotificationId);
                throw;
            }
        }

        private int GetPriorityFromId(Guid priorityId)
        {
            // This is a simplified mapping - in a real application, you'd query the database
            // or use a lookup service to get the actual priority value
            return 1; // Normal priority
        }
    }
}