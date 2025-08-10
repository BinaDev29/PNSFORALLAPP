// File Path: Application/CQRS/NotificationType/Queries/GetNotificationTypeDetailQuery.cs
using Application.DTO.NotificationType;
using MediatR;
using System;

namespace Application.CQRS.NotificationType.Queries
{
    public class GetNotificationTypeDetailQuery : IRequest<NotificationTypeDto>
    {
        public Guid Id { get; set; }
    }
}