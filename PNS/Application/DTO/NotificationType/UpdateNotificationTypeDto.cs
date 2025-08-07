using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.NotificationType
{
    public class UpdateNotificationTypeDto
    {
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Subject { get; set; }
    }
}