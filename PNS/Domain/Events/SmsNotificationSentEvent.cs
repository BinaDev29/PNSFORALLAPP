// File Path: Domain/Events/SmsNotificationSentEvent.cs
using Domain.Common;
using System;

namespace Domain.Events
{
    public class SmsNotificationSentEvent : DomainEvent
    {
        public Guid NotificationId { get; }
        public string PhoneNumber { get; }
        public string Message { get; }
        public string? MessageId { get; }
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }

        public SmsNotificationSentEvent(Guid notificationId, string phoneNumber, string message,
            string? messageId = null, bool isSuccess = true, string? errorMessage = null)
        {
            NotificationId = notificationId;
            PhoneNumber = phoneNumber;
            Message = message;
            MessageId = messageId;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}