// File Path: Application/CQRS/Notification/Queries/GetNotificationsListQuery.cs
using Application.DTO.Notification;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.Notification.Queries
{
    public class GetNotificationsListQuery : IRequest<List<NotificationDto>>
    {
        public string? UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}