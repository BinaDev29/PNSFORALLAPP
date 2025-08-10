// File Path: Domain/Models/NotificationType.cs
using Domain.Common;
using System.Collections.Generic;

namespace Domain.Models
{
    public class NotificationType : BaseDomainEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<ApplicationNotificationTypeMap> ApplicationNotificationTypeMaps { get; set; } = new List<ApplicationNotificationTypeMap>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}