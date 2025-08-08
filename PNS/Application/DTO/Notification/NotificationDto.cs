using System;

namespace Application.DTO.Notification
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid ClientApplicationId { get; set; }
        public Guid NotificationTypeId { get; set; }
        public required string Recipient { get; set; }
        public string? RecipientIp { get; set; } // ተጨምሯል
        public string? RecipientDeviceType { get; set; } // ተጨምሯል
        public string? Sender { get; set; } // ተጨምሯል
        public required string Title { get; set; }
        public required string Message { get; set; }
        public Guid PriorityId { get; set; } // ተጨምሯል
        public DateTime? ReceivedAt { get; set; } // ተጨምሯል
        public DateTime? SeenAt { get; set; } // ተጨምሯል
    }
}