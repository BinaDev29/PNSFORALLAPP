// File Path: Application/CQRS/NotificationType/Commands/DeleteNotificationTypeCommand.cs
using MediatR;
using System;

namespace Application.CQRS.NotificationType.Commands
{
    public class DeleteNotificationTypeCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}