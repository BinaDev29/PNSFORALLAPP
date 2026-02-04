// File Path: Application/DTO/NotificationHistory/NotificationHistoryDto.cs
using System;

namespace Application.DTO.NotificationHistory
{
    public class NotificationHistoryDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid NotificationId { get; set; }
        public DateTime SentDate { get; set; }
        
        // Enriched properties from Notification
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty; // Single recipient or comma separated
        public string NotificationType { get; set; } = string.Empty;
        
        public string? ErrorMessage { get; set; }
    }
}