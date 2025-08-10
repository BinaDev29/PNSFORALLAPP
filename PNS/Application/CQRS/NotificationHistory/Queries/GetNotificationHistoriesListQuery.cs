// File Path: Application/CQRS/NotificationHistory/Queries/GetNotificationHistoriesListQuery.cs
using Application.DTO.NotificationHistory;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.NotificationHistory.Queries
{
    public class GetNotificationHistoriesListQuery : IRequest<List<NotificationHistoryDto>>
    {
    }
}