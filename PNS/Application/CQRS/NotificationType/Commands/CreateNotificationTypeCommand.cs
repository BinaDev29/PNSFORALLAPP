using MediatR;
using Application.DTO.NotificationType;
using Application.Responses;

namespace Application.CQRS.NotificationType.Commands
{
    public class CreateNotificationTypeCommand : IRequest<BaseCommandResponse>
    {
        public required CreateNotificationTypeDto CreateNotificationTypeDto { get; set; }
    }
}