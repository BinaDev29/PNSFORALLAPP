// File Path: Application/CQRS/Notification/Commands/UpdateNotificationCommand.cs
using Application.DTO.Notification;
using MediatR;

namespace Application.CQRS.Notification.Commands
{
    public class UpdateNotificationCommand : IRequest<Unit>
    {
        public required UpdateNotificationDto UpdateNotificationDto { get; set; }
    }
}