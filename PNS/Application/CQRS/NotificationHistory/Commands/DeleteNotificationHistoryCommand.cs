using MediatR;
using System;

namespace Application.CQRS.NotificationHistory.Commands
{
    public class DeleteNotificationHistoryCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}