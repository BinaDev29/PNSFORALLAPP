using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.NotificationHistory
{
    public class UpdateNotificationHistoryDto
    {
        public Guid Id { get; set; }
        [Required]
        public required string Status { get; set; }
        public Guid NotificationId { get; set; }
    }
}