using System;

namespace Domain.Common
{
    public abstract class BaseDomainEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Default ዋጋ ታክሏል
        public string CreatedBy { get; set; } = "SYSTEM"; // Default ዋጋ ታክሏል
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}