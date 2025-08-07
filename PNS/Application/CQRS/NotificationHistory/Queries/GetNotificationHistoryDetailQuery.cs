using MediatR;
using Application.DTO.NotificationHistory;
using System;

namespace Application.CQRS.NotificationHistory.Queries
{
    public class GetNotificationHistoryDetailQuery : IRequest<NotificationHistoryDto>
    {
        public Guid Id { get; set; }
    }
}