using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.NotificationType
{
    public class CreateNotificationTypeDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Subject { get; set; }
    }
}