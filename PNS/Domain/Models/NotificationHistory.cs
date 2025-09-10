// File Path: Domain/Models/NotificationHistory.cs
using Domain.Common;
using System;

namespace Domain.Models
{
    public class NotificationHistory : BaseDomainEntity
    {
        public required string Status { get; set; }
        public Guid? NotificationId { get; set; } // ወደ Nullable ተቀይሯል
        public required DateTime SentDate { get; set; }
        public virtual Notification? Notification { get; set; }
    }
}