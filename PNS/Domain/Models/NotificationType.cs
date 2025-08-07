using Domain.Common;
using System.Collections.Generic;

namespace Domain.Models
{
    public class NotificationType : BaseDomainEntity
    {
        public required string Name { get; set; } // 'required' ተጨምሯል
        public required string Description { get; set; } // 'required' ተጨምሯል

        // Navigation Properties
        public ICollection<ApplicationNotificationTypeMap> ApplicationNotificationTypeMaps { get; set; } = new List<ApplicationNotificationTypeMap>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}