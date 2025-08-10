// File Path: Application/DTO/ApplicationNotificationTypeMap/CreateApplicationNotificationTypeMapDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.ApplicationNotificationTypeMap
{
    public class CreateApplicationNotificationTypeMapDto
    {
        [Required]
        public Guid ClientApplicationId { get; set; }
        [Required]
        public Guid NotificationTypeId { get; set; }
    }
}