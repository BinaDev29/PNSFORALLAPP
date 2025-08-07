using MediatR;
using Application.DTO.NotificationHistory;
using System.Collections.Generic;

namespace Application.CQRS.NotificationHistory.Queries
{
    public class GetNotificationHistoriesListQuery : IRequest<IReadOnlyList<NotificationHistoryDto>>
    {
    }
}