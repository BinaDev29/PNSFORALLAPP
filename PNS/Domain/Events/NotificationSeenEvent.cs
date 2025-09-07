// File Path: Domain/Events/NotificationSeenEvent.cs
using Domain.Common;
using System;

namespace Domain.Events
{
    public class NotificationSeenEvent : DomainEvent
    {
        public Guid NotificationId { get; private set; }
        public DateTime SeenAt { get; private set; }
        public string? UserAgent { get; private set; }
        public string? IpAddress { get; private set; }

        public NotificationSeenEvent(Guid notificationId, DateTime seenAt, string? userAgent = null, string? ipAddress = null)
        {
            NotificationId = notificationId;
            SeenAt = seenAt;
            UserAgent = userAgent;
            IpAddress = ipAddress;
        }
    }
}