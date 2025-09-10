// File Path: Application/DTO/NotificationType/NotificationTypeDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.NotificationType
{
    public class NotificationTypeDto
    {
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}