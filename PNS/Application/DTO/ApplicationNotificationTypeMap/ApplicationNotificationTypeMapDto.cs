// File Path: Application/DTO/ApplicationNotificationTypeMap/ApplicationNotificationTypeMapDto.cs
using System;

namespace Application.DTO.ApplicationNotificationTypeMap
{
    public class ApplicationNotificationTypeMapDto
    {
        public Guid Id { get; set; }
        public Guid ClientApplicationId { get; set; }
        public Guid NotificationTypeId { get; set; }
    }
}