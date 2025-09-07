// File Path: Domain/Common/IAuditableEntity.cs
using System;

namespace Domain.Common
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime? LastModifiedDate { get; set; }
        string? LastModifiedBy { get; set; }
    }
}