// File Path: Application/DTO/Notification/UpdateNotificationDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Notification
{
    public class UpdateNotificationDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public required Guid ClientApplicationId { get; set; }
        [Required]
        public required string To { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Message { get; set; }
        [Required]
        public required Guid NotificationTypeId { get; set; }
        [Required]
        public required Guid PriorityId { get; set; }
    }
}