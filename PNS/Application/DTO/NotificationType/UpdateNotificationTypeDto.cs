// File Path: Application/DTO/NotificationType/UpdateNotificationTypeDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.NotificationType
{
    public class UpdateNotificationTypeDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}