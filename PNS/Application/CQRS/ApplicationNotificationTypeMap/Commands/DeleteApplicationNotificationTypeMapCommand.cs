// File Path: Application/CQRS/ApplicationNotificationTypeMap/Commands/DeleteApplicationNotificationTypeMapCommand.cs
using MediatR;
using System;

namespace Application.CQRS.ApplicationNotificationTypeMap.Commands
{
    public class DeleteApplicationNotificationTypeMapCommand : IRequest<Unit>
    {
        public Guid ClientApplicationId { get; set; }
        public Guid NotificationTypeId { get; set; }
    }
}