using MediatR;
using Application.DTO.NotificationHistory;

namespace Application.CQRS.NotificationHistory.Commands
{
    public class UpdateNotificationHistoryCommand : IRequest<Unit>
    {
        public required UpdateNotificationHistoryDto UpdateNotificationHistoryDto { get; set; }
    }
}