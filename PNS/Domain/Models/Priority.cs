// File Path: Domain/Models/Priority.cs
using Domain.Common;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Priority : BaseDomainEntity
    {
        public required string Description { get; set; }
        public int Level { get; set; }

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}