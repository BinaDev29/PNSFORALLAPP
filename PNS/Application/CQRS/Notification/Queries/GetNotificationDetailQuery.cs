using MediatR;
using Application.DTO.Notification;
using System;

namespace Application.CQRS.Notification.Queries
{
    public class GetNotificationDetailQuery : IRequest<NotificationDto>
    {
        public Guid Id { get; set; }
    }
}