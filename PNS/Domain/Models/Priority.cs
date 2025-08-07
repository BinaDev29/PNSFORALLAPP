using Domain.Common;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Priority : BaseDomainEntity
    {
        public required string Description { get; set; } // 'required' ተጨምሯል
        public int Level { get; set; }

        // Navigation Properties
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}