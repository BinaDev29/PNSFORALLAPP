// File Path: Domain/Models/NotificationHistory.cs
using Domain.Common;
using System;

namespace Domain.Models
{
    public class NotificationHistory : BaseDomainEntity
    {
        public required string Status { get; set; }
        public Guid? NotificationId { get; set; } 
        public required DateTime SentDate { get; set; }
        public string? Recipient { get; set; }
        public string? NotificationType { get; set; }
        public string? ErrorMessage { get; set; }
        public virtual Notification? Notification { get; set; }
    }
}