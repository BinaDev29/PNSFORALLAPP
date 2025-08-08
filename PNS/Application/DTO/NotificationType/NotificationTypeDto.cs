using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.NotificationType
{
    public class NotificationTypeDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Subject { get; set; }
        public required string Description { get; set; } // Description ተጨምሯል
    }
}