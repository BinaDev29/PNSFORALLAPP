// File Path: Domain/Models/Notification.cs
using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class Notification : AggregateRoot
    {
        public required Guid ClientApplicationId { get; set; }
        public virtual ClientApplication? ClientApplication { get; set; }

        public required List<string> To { get; set; }
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
        
        // Enhanced properties
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
        public int RetryCount { get; set; } = 0;
        public int MaxRetries { get; set; } = 3;
        public DateTime? ScheduledAt { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }

        // Business methods
        public void MarkAsSeen(string? userAgent = null, string? ipAddress = null)
        {
            if (SeenTime.HasValue) return; // Already seen

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

        public List<EmailAddress> GetValidEmailAddresses()
        {
            var validEmails = new List<EmailAddress>();
            
            foreach (var email in To)
            {
                try
                {
                    validEmails.Add(EmailAddress.Create(email));
                }
                catch
                {
                    // Skip invalid emails
                }
            }
            
            return validEmails;
        }

        public static Notification Create(Guid clientApplicationId, List<string> recipients, 
            string title, string message, Guid notificationTypeId, Guid priorityId, 
            DateTime? scheduledAt = null)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                ClientApplicationId = clientApplicationId,
                To = recipients,
                Title = title,
                Message = message,
                NotificationTypeId = notificationTypeId,
                PriorityId = priorityId,
                Status = scheduledAt.HasValue ? NotificationStatus.Scheduled : NotificationStatus.Pending,
                ScheduledAt = scheduledAt
            };

            notification.AddDomainEvent(new NotificationCreatedEvent(
                notification.Id, clientApplicationId, recipients, title, message, 
                notificationTypeId, priorityId));

            return notification;
        }
    }

    public enum NotificationStatus
    {
        Pending = 0,
        Scheduled = 1,
        Sending = 2,
        Sent = 3,
        Seen = 4,
        Failed = 5,
        Cancelled = 6
    }
}