// File Path: Application/DTO/NotificationHistory/CreateNotificationHistoryDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.NotificationHistory
{
    public class CreateNotificationHistoryDto
    {
        [Required]
        public required string Status { get; set; } = "Pending";
        [Required]
        public required Guid NotificationId { get; set; }
    }
}