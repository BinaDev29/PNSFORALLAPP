// File Path: Domain/Models/Notification.cs
using Domain.Common;
using Domain.Events;
using Domain.Enums; // ከዚህ ነው የምትጨምረው
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class Notification : AggregateRoot
    {
        public Guid ClientApplicationId { get; set; }
        public virtual ClientApplication? ClientApplication { get; set; }

        public required List<object> To { get; set; }
        public DateTime? ReceivedTime { get; set; }
        public DateTime? SeenTime { get; set; }
        public string? Secret { get; set; }
        public string? IP { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required Guid PriorityId { get; set; }
        public virtual Priority? Priority { get; set; }
        public required Guid NotificationTypeId { get; set; }
        public virtual NotificationType? NotificationType { get; set; }

        // አዲስ የጨመርከው ግንኙነት
        public ICollection<NotificationHistory> NotificationHistories { get; set; } = new List<NotificationHistory>();

        // Enhanced properties
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
        public int RetryCount { get; set; } = 0;
        public int MaxRetries { get; set; } = 3;
        public DateTime? ScheduledAt { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }

        private Notification() { }

        // Factory Method
        public static Notification CreateNotification(
            Guid clientApplicationId, List<string> recipients,
            string title, string message, Guid priorityId, Guid notificationTypeId,
            DateTime? scheduledAt = null, Dictionary<string, string>? metadata = null)
        {
            // ይህንን ክፍል በApplication Layer ውስጥ መቆጣጠር የተሻለ ነው።
            // This part of the logic is better handled in the Application Layer.
            // Domain Layer should only deal with domain-specific logic.
            var validRecipients = recipients.Select(r => (object)EmailAddress.Create(r)).ToList();

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                ClientApplicationId = clientApplicationId,
                To = validRecipients,
                Title = title,
                Message = message,
                NotificationTypeId = notificationTypeId,
                PriorityId = priorityId,
                Status = scheduledAt.HasValue ? NotificationStatus.Scheduled : NotificationStatus.Pending,
                ScheduledAt = scheduledAt,
                Metadata = metadata
            };

            notification.AddDomainEvent(new NotificationCreatedEvent(
                notification.Id, clientApplicationId, notification.To, title, message,
                notification.NotificationTypeId, priorityId));

            return notification;
        }

        // ... Business methods
        public void MarkAsSeen(string? userAgent = null, string? ipAddress = null)
        {
            if (SeenTime.HasValue) return;
            SeenTime = DateTime.UtcNow;
            Status = NotificationStatus.Seen;
            IP = ipAddress;
            AddDomainEvent(new NotificationSeenEvent(Id, SeenTime.Value, userAgent, ipAddress));
        }

        public void MarkAsSent()
        {
            Status = NotificationStatus.Sent;
            ReceivedTime = DateTime.UtcNow;
        }

        public void MarkAsFailed(string errorMessage)
        {
            Status = NotificationStatus.Failed;
            ErrorMessage = errorMessage;
            RetryCount++;
        }

        public bool CanRetry()
        {
            return RetryCount < MaxRetries && Status == NotificationStatus.Failed;
        }

        public void ScheduleFor(DateTime scheduledTime)
        {
            ScheduledAt = scheduledTime;
            Status = NotificationStatus.Scheduled;
        }

        public bool IsReadyToSend()
        {
            return Status == NotificationStatus.Pending ||
                   (Status == NotificationStatus.Scheduled && ScheduledAt <= DateTime.UtcNow);
        }
    }
}