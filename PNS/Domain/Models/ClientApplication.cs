using Domain.Common;
using System.Collections.Generic;

namespace Domain.Models
{
    public class ClientApplication : BaseDomainEntity
    {
        public required string AppId { get; set; }
        public required string Name { get; set; }
        public string? Slogan { get; set; }
        public string? Logo { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<ApplicationNotificationTypeMap> ApplicationNotificationTypeMaps { get; set; } = new List<ApplicationNotificationTypeMap>();
    }
}