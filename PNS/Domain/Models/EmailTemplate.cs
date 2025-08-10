// File Path: Domain/Models/EmailTemplate.cs
using Domain.Common;
using System;

namespace Domain.Models
{
    public class EmailTemplate : BaseDomainEntity
    {
        public required string Name { get; set; }
        public required string Subject { get; set; }
        public required string BodyHtml { get; set; }
        public string? BodyText { get; set; }
    }
}