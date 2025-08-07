using MediatR;
using Application.DTO.NotificationType;
using System;

namespace Application.CQRS.NotificationType.Queries
{
    public class GetNotificationTypeDetailQuery : IRequest<NotificationTypeDto>
    {
        public Guid Id { get; set; }
    }
}