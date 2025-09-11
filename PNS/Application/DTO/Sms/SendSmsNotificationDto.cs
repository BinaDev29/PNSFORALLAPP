// File Path: Application/DTO/Sms/SendSmsNotificationDto.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Sms
{
    public class SendSmsNotificationDto
    {
        [Required]
        public Guid ClientApplicationId { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(1600)] // SMS character limit
        public required string Message { get; set; }

        [Required]
        public Guid PriorityId { get; set; }

        [Required]
        public Guid NotificationTypeId { get; set; }

        public DateTime? ScheduledAt { get; set; }
        public string? TemplateName { get; set; }
        public Dictionary<string, string>? TemplateData { get; set; }
    }
}