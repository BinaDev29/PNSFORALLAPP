using Application.Contracts;
using Application.Contracts.IRepository;
using Application.DTO.Webhook;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EventHandlers
{
    public class WebhookEventHandler : 
        INotificationHandler<NotificationSeenEvent>,
        INotificationHandler<SmsNotificationSentEvent>,
        INotificationHandler<EmailNotificationSentEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebhookService _webhookService;
        private readonly ILogger<WebhookEventHandler> _logger;

        public WebhookEventHandler(
            IUnitOfWork unitOfWork,
            IWebhookService webhookService,
            ILogger<WebhookEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _webhookService = webhookService;
            _logger = logger;
        }

        public async Task Handle(NotificationSeenEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var notificationEntity = await _unitOfWork.Notifications.Get(notification.NotificationId);
                if (notificationEntity == null) return;

                var clientApp = await _unitOfWork.ClientApplications.Get(notificationEntity.ClientApplicationId);
                if (clientApp == null || string.IsNullOrEmpty(clientApp.WebhookUrl)) return;

                var payload = new WebhookPayload
                {
                    NotificationId = notification.NotificationId,
                    EventType = "Seen",
                    Recipient = notificationEntity.To?[0]?.ToString() ?? "Unknown",
                    Timestamp = notification.SeenAt,
                    Metadata = $"IP: {notification.IpAddress}, UA: {notification.UserAgent}"
                };

                await _webhookService.SendWebhookAsync(clientApp.WebhookUrl, clientApp.WebhookSecret ?? "", payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling NotificationSeenEvent for Webhook");
            }
        }

        public async Task Handle(SmsNotificationSentEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var notificationEntity = await _unitOfWork.Notifications.Get(notification.NotificationId);
                if (notificationEntity == null) return;

                var clientApp = await _unitOfWork.ClientApplications.Get(notificationEntity.ClientApplicationId);
                if (clientApp == null || string.IsNullOrEmpty(clientApp.WebhookUrl)) return;

                var payload = new WebhookPayload
                {
                    NotificationId = notification.NotificationId,
                    EventType = notification.IsSuccess ? "Sent" : "Failed",
                    Recipient = notification.PhoneNumber,
                    Timestamp = DateTime.UtcNow,
                    Metadata = notification.IsSuccess ? $"MessageId: {notification.MessageId}" : $"Error: {notification.ErrorMessage}"
                };

                await _webhookService.SendWebhookAsync(clientApp.WebhookUrl, clientApp.WebhookSecret ?? "", payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling SmsNotificationSentEvent for Webhook");
            }
        }

        public async Task Handle(EmailNotificationSentEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var notificationEntity = await _unitOfWork.Notifications.Get(notification.NotificationId);
                if (notificationEntity == null) return;

                var clientApp = await _unitOfWork.ClientApplications.Get(notificationEntity.ClientApplicationId);
                if (clientApp == null || string.IsNullOrEmpty(clientApp.WebhookUrl)) return;

                var payload = new WebhookPayload
                {
                    NotificationId = notification.NotificationId,
                    EventType = notification.IsSuccess ? "Sent" : "Failed",
                    Recipient = notification.Recipient,
                    Timestamp = DateTime.UtcNow,
                    Metadata = notification.IsSuccess ? "Email sent via background processor" : $"Error: {notification.ErrorMessage}"
                };

                await _webhookService.SendWebhookAsync(clientApp.WebhookUrl, clientApp.WebhookSecret ?? "", payload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling EmailNotificationSentEvent for Webhook");
            }
        }
    }
}
