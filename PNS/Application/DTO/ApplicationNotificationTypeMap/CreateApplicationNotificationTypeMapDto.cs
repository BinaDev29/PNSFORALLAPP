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
        public bool IsEnabled { get; set; }
        public Guid? EmailTemplateId { get; set; }
    }
}