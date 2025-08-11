// File Path: Application/CQRS/Notification/Queries/GetUnseenNotificationsQuery.cs
using Application.DTO.Notification;
using MediatR;
using System.Collections.Generic;
using System;

namespace Application.CQRS.Notification.Queries
{
    public class GetUnseenNotificationsQuery : IRequest<List<NotificationDto>>
    {
        public Guid ClientApplicationId { get; set; }
    }
}