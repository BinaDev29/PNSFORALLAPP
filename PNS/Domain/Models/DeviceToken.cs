using Domain.Common;
using System;

namespace Domain.Models
{
    public class DeviceToken : BaseDomainEntity
    {
        public string Token { get; set; } = string.Empty;
        public string? UserId { get; set; } // Optional user identification
        public string? DeviceType { get; set; } // "Web", "Android", "iOS"
        public Guid ClientApplicationId { get; set; }
        public virtual ClientApplication? ClientApplication { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
