using Domain.Common;
using System;
using Domain.Models; // ይህ ለClientApplication, NotificationType, እና EmailTemplate ያስፈልጋል

namespace Domain.Models
{
    public class ApplicationNotificationTypeMap : BaseDomainEntity
    {
        public Guid ClientApplicationId { get; set; }
        public virtual ClientApplication? ClientApplication { get; set; }

        public Guid NotificationTypeId { get; set; }
        public virtual NotificationType? NotificationType { get; set; }

        public bool IsEnabled { get; set; }
        public Guid? EmailTemplateId { get; set; }
        public virtual EmailTemplate? EmailTemplate { get; set; }
    }
}