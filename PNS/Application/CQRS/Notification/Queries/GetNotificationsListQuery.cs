using MediatR;
using Application.DTO.Notification;
using System.Collections.Generic;

namespace Application.CQRS.Notification.Queries
{
    public class GetNotificationsListQuery : IRequest<IReadOnlyList<NotificationDto>>
    {
    }
}