using MediatR;
using Application.DTO.Notification;

namespace Application.CQRS.Notification.Commands
{
    public class UpdateNotificationCommand : IRequest<Unit>
    {
        public required UpdateNotificationDto UpdateNotificationDto { get; set; }
    }
}