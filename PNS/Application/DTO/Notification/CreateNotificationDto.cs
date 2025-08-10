// File Path: Application/DTO/Notification/CreateNotificationDto.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Notification
{
    public class CreateNotificationDto
    {
        [Required]
        public Guid ClientApplicationId { get; set; }

        public required List<string> To { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Message { get; set; }

        [Required]
        public Guid NotificationTypeId { get; set; }

        [Required]
        public Guid PriorityId { get; set; }
    }
}