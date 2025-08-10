// File Path: Domain/Models/Notification.cs
using Domain.Common;
using System;
using System.Collections.Generic; // ለ'List<string>' ያስፈልጋል

namespace Domain.Models
{
    public class Notification : BaseDomainEntity
    {
        public required Guid ClientApplicationId { get; set; }
        public virtual ClientApplication? ClientApplication { get; set; }

        public required List<string> To { get; set; } // 'string' ወደ 'List<string>' ተቀይሯል
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
    }
}