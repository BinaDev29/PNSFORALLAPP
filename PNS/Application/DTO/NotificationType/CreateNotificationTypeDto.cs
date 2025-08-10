// File Path: Application/DTO/NotificationType/CreateNotificationTypeDto.cs
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.NotificationType
{
    public class CreateNotificationTypeDto
    {
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}