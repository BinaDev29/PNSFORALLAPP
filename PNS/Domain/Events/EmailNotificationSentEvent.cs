using Domain.Common;
using System;

namespace Domain.Events
{
    public class EmailNotificationSentEvent : DomainEvent
    {
        public Guid NotificationId { get; }
        public string Recipient { get; }
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }

        public EmailNotificationSentEvent(Guid notificationId, string recipient, bool isSuccess = true, string? errorMessage = null)
        {
            NotificationId = notificationId;
            Recipient = recipient;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
