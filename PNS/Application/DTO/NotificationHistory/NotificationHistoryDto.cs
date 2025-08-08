using System;
using System.ComponentModel.DataAnnotations;
using Application.DTO.Notification;

namespace Application.DTO.NotificationHistory
{
    public class NotificationHistoryDto
    {
        public Guid Id { get; set; }
        public required string Status { get; set; }
        public Guid NotificationId { get; set; }
        public NotificationDto Notification { get; set; }
    }
}