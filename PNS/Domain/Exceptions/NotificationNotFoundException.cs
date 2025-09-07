// File Path: Domain/Exceptions/NotificationNotFoundException.cs
using System;

namespace Domain.Exceptions
{
    public class NotificationNotFoundException : DomainException
    {
        public NotificationNotFoundException(Guid notificationId) 
            : base($"Notification with ID '{notificationId}' was not found.")
        {
        }
    }
}