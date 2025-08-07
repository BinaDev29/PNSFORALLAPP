using MediatR;
using System;

namespace Application.CQRS.Notification.Commands
{
    public class DeleteNotificationCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}