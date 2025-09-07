// File Path: Domain/Events/NotificationCreatedEvent.cs
using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Events
{
    public class NotificationCreatedEvent : DomainEvent
    {
        public Guid NotificationId { get; private set; }
        public Guid ClientApplicationId { get; private set; }
        public List<string> Recipients { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public Guid NotificationTypeId { get; private set; }
        public Guid PriorityId { get; private set; }

        public NotificationCreatedEvent(Guid notificationId, Guid clientApplicationId, 
            List<string> recipients, string title, string message, 
            Guid notificationTypeId, Guid priorityId)
        {
            NotificationId = notificationId;
            ClientApplicationId = clientApplicationId;
            Recipients = recipients;
            Title = title;
            Message = message;
            NotificationTypeId = notificationTypeId;
            PriorityId = priorityId;
        }
    }
}