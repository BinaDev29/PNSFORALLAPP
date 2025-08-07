using MediatR;
using Application.DTO.NotificationHistory;
using Application.Responses;

namespace Application.CQRS.NotificationHistory.Commands
{
    public class CreateNotificationHistoryCommand : IRequest<BaseCommandResponse>
    {
        public required CreateNotificationHistoryDto CreateNotificationHistoryDto { get; set; }
    }
}