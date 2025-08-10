// File Path: Application/DTO/NotificationType/NotificationTypeDto.cs
using System;

namespace Application.DTO.NotificationType
{
    public class NotificationTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}