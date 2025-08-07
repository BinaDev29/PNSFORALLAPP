using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Notification
{
    public class UpdateNotificationDto
    {
        public Guid Id { get; set; }
        [Required]
        public required string Recipient { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Message { get; set; }
        public required string Status { get; set; }
        public Guid? ClientApplicationId { get; set; }
    }
}