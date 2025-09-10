// File Path: Application/CQRS/NotificationType/Commands/UpdateNotificationTypeCommand.cs
using Application.DTO.NotificationType;
using MediatR;
using System;

namespace Application.CQRS.NotificationType.Commands
{
    public class UpdateNotificationTypeCommand : IRequest<Unit>
    {
        public required UpdateNotificationTypeDto UpdateNotificationTypeDto { get; set; }
    }
}