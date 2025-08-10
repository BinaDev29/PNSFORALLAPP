// File Path: Application/DTO/Notification/NotificationDto.cs
using System;
using System.Collections.Generic;

namespace Application.DTO.Notification
{
    public class NotificationDto
    {
        public required Guid Id { get; set; }
        public required Guid ClientApplicationId { get; set; }
        public required List<string> To { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required Guid NotificationTypeId { get; set; }
        public required Guid PriorityId { get; set; }
    }
}