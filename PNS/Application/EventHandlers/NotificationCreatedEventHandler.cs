// File Path: Application/EventHandlers/NotificationCreatedEventHandler.cs
using Application.Common.Interfaces;
using Application.Models.Email;
using Application.Services;
using Application.Contracts;
using Application.Contracts.IRepository;
using Domain.Events;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EventHandlers
{
    public class NotificationCreatedEventHandler : INotificationHandler<NotificationCreatedEvent>
    {
        private readonly IEmailQueueService _emailQueueService;
        private readonly ISmsQueueService _smsQueueService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationCreatedEventHandler> _logger;

        public NotificationCreatedEventHandler(
            IEmailQueueService emailQueueService,
            ISmsQueueService smsQueueService,
            IEmailTemplateService emailTemplateService,
            IMessageQueueService messageQueueService,
            IPushNotificationService pushNotificationService,
            IUnitOfWork unitOfWork,
            ILogger<NotificationCreatedEventHandler> logger)
        {
            _emailQueueService = emailQueueService;
            _smsQueueService = smsQueueService;
            _emailTemplateService = emailTemplateService;
            _messageQueueService = messageQueueService;
            _pushNotificationService = pushNotificationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(NotificationCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing NotificationCreatedEvent for notification {NotificationId}",
                    notification.NotificationId);

                // 1. Publish to Message Queue (RabbitMQ)
                await _messageQueueService.PublishMessageAsync("notification_created", notification);

                // 2. Send Push Notification to all registered devices for this Client App
                var deviceTokens = await _unitOfWork.DeviceTokens.GetAll();
                var clientTokens = deviceTokens.Where(t => t.ClientApplicationId == notification.ClientApplicationId).Select(t => t.Token).ToList();
                if (clientTokens.Any())
                {
                    await _pushNotificationService.SendBatchPushAsync(clientTokens, notification.Title, notification.Message);
                }

                // 3. Process by Notification Type (Email/SMS)
                var notificationType = await _unitOfWork.NotificationTypes.Get(notification.NotificationTypeId);
                
                if (notificationType?.Name == "Email")
                {
                    var processedHtml = await _emailTemplateService.ProcessNotificationTemplateAsync(notification.NotificationId);
                    var emailMessage = new EnhancedEmailMessage
                    {
                        From = "noreply@pns-hub.com",
                        To = notification.Recipients.Select(r => r.ToString()).ToList(),
                        Subject = notification.Title,
                        BodyHtml = processedHtml,
                        TrackingId = notification.NotificationId.ToString() + "_email",
                        EnableTracking = true
                    };
                    await _emailQueueService.QueueEmailAsync(emailMessage, GetPriorityFromId(notification.PriorityId));
                }
                else if (notificationType?.Name == "SMS")
                {
                    foreach (var recipient in notification.Recipients)
                    {
                        var smsMessage = new SmsMessage
                        {
                            To = recipient.ToString() ?? "",
                            Body = notification.Message,
                            TrackingId = notification.NotificationId.ToString() + "_" + recipient.ToString(),
                            RetryCount = 0,
                            MaxRetries = 3
                        };
                        await _smsQueueService.EnqueueSmsAsync(smsMessage);
                    }
                }

                _logger.LogInformation("Successfully processed notification {NotificationId}", notification.NotificationId);
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
            return 1; // Normal priority
        }
    }
}