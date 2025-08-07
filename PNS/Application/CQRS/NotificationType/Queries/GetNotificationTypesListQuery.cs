using MediatR;
using Application.DTO.NotificationType;
using System.Collections.Generic;

namespace Application.CQRS.NotificationType.Queries
{
    public class GetNotificationTypesListQuery : IRequest<IReadOnlyList<NotificationTypeDto>>
    {
    }
}