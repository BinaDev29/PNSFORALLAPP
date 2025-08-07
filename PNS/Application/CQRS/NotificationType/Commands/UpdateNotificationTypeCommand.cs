using MediatR;
using Application.DTO.NotificationType;

namespace Application.CQRS.NotificationType.Commands
{
    public class UpdateNotificationTypeCommand : IRequest<Unit>
    {
        public required UpdateNotificationTypeDto UpdateNotificationTypeDto { get; set; }
    }
}