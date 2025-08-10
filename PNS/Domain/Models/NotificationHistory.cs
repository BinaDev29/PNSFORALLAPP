// File Path: Domain/Models/NotificationHistory.cs
using Domain.Common;
using System;

namespace Domain.Models
{
    public class NotificationHistory : BaseDomainEntity
    {
        public string Status { get; set; } = string.Empty;
        public Guid NotificationId { get; set; }
        public DateTime SentDate { get; set; }
        public virtual Notification? Notification { get; set; }
    }
}