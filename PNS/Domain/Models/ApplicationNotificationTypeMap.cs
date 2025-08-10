// File Path: Domain/Models/ApplicationNotificationTypeMap.cs
using Domain.Common;
using System;

namespace Domain.Models
{
    public class ApplicationNotificationTypeMap : BaseDomainEntity
    {
        public Guid ClientApplicationId { get; set; }
        public virtual ClientApplication? ClientApplication { get; set; }

        public Guid NotificationTypeId { get; set; }
        public virtual NotificationType? NotificationType { get; set; }
    }
}