// File Path: Application/CQRS/Notification/Queries/GetNotificationDetailQuery.cs
using Application.DTO.Notification;
using MediatR;
using System;

namespace Application.CQRS.Notification.Queries
{
    public class GetNotificationDetailQuery : IRequest<NotificationDto>
    {
        public Guid Id { get; set; }
    }
}