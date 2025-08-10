// File Path: Application/DTO/NotificationHistory/NotificationHistoryDto.cs
using System;

namespace Application.DTO.NotificationHistory
{
    public class NotificationHistoryDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public Guid NotificationId { get; set; }
        public DateTime SentDate { get; set; }
    }
}