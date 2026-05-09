using System;

namespace Application.DTO.Webhook
{
    public class WebhookPayload
    {
        public Guid NotificationId { get; set; }
        public string EventType { get; set; } // "Sent", "Seen", "Failed"
        public string Recipient { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Metadata { get; set; }
    }
}
