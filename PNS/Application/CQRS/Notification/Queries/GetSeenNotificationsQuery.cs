// File Path: Application/CQRS/Notification/Queries/GetSeenNotificationsQuery.cs
using Application.DTO.Notification;
using MediatR;
using System.Collections.Generic;
using System;

namespace Application.CQRS.Notification.Queries
{
    public class GetSeenNotificationsQuery : IRequest<List<NotificationDto>>
    {
        public Guid ClientApplicationId { get; set; }
    }
}