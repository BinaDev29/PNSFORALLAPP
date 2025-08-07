using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class EmailTemplate : BaseDomainEntity
    {
        public required string Name { get; set; } // 'required' ተጨምሯል
        public required string Subject { get; set; } // 'required' ተጨምሯል
        public required string BodyHtml { get; set; } // 'required' ተጨምሯል
        public string? BodyText { get; set; }
    }
}