using System;

namespace Application.DTO.Notification
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid ClientApplicationId { get; set; }
        public Guid NotificationTypeId { get; set; }
        public string Recipient { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}