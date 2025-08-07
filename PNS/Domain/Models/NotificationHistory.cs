// NotificationHistory.cs
using Domain.Common;
using System;
using Domain.Models;

namespace Domain.Models
{
    public class NotificationHistory : BaseDomainEntity
    {
        public string Status { get; set; } = string.Empty;
        public Guid NotificationId { get; set; }
        public DateTime SentDate { get; set; } // ይህን ጨምር
        public virtual Notification? Notification { get; set; }
    }
}