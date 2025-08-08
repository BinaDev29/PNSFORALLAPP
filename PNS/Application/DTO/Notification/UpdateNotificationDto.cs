using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Notification
{
    public class UpdateNotificationDto
    {
        [Required]
        public Guid Id { get; set; }
        public required string Status { get; set; }
        public DateTime? SeenAt { get; set; } // ይህን መጨመር የnotificationው መነበቡን ለመመዝገብ ይጠቅማል
    }
}