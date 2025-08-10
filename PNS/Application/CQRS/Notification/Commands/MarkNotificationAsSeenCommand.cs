// File Path: Application/CQRS/Notification/Commands/MarkNotificationAsSeenCommand.cs
using MediatR;
using System;

namespace Application.CQRS.Notification.Commands
{
    public class MarkNotificationAsSeenCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}