// File Path: Application/CQRS/Notification/Commands/CreateNotificationCommand.cs
using Application.DTO.Notification;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Notification.Commands
{
    public class CreateNotificationCommand : IRequest<BaseCommandResponse>
    {
        public required CreateNotificationDto CreateNotificationDto { get; set; }
        public string? UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}