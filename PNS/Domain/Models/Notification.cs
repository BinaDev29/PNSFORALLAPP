// File Path: Domain/Models/Notification.cs
using Domain.Common;
using System;

namespace Domain.Models
{
    public class Notification : BaseDomainEntity
    {
        public Guid ClientApplicationId { get; set; }
        public virtual ClientApplication? ClientApplication { get; set; }

        public Guid NotificationTypeId { get; set; }
        public virtual NotificationType? NotificationType { get; set; }

        public required string Recipient { get; set; } // 'required' ተጨምሯል
        public string? RecipientIp { get; set; }
        public string? RecipientDeviceType { get; set; }
        public string? Sender { get; set; }

        public required string Title { get; set; } // 'required' ተጨምሯል
        public required string Message { get; set; } // 'required' ተጨምሯል

        public Guid PriorityId { get; set; }
        public virtual Priority? Priority { get; set; }

        public DateTime? ReceivedAt { get; set; }
        public DateTime? SeenAt { get; set; }
    }
}