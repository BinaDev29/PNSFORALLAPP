// File Path: Domain/Common/BaseDomainEntity.cs
using System;

namespace Domain.Common
{
    public abstract class BaseDomainEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "SYSTEM";
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}