// File Path: Application/CQRS/NotificationType/Commands/CreateNotificationTypeCommand.cs
using Application.DTO.NotificationType;
using Application.Responses;
using MediatR;

namespace Application.CQRS.NotificationType.Commands
{
    public class CreateNotificationTypeCommand : IRequest<BaseCommandResponse>
    {
        public required CreateNotificationTypeDto CreateNotificationTypeDto { get; set; }
    }
}