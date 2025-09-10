// File Path: Domain/Models/SmsTemplate.cs
using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class SmsTemplate : BaseDomainEntity
    {
        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }

        [Required]
        public required string Body { get; set; }
    }
}