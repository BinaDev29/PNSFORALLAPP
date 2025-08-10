// File Path: Application/CQRS/NotificationHistory/Queries/GetNotificationHistoryDetailQuery.cs
using Application.DTO.NotificationHistory;
using MediatR;
using System;

namespace Application.CQRS.NotificationHistory.Queries
{
    public class GetNotificationHistoryDetailQuery : IRequest<NotificationHistoryDto>
    {
        public Guid Id { get; set; }
    }
}