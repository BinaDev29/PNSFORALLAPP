using MediatR;
using Application.DTO.Notification;
using Application.Responses;

namespace Application.CQRS.Notification.Commands
{
    public class CreateNotificationCommand : IRequest<BaseCommandResponse>
    {
        public required CreateNotificationDto CreateNotificationDto { get; set; }
    }
}