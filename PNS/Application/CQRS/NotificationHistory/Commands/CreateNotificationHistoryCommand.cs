// File Path: Application/CQRS/NotificationHistory/Commands/CreateNotificationHistoryCommand.cs
using Application.DTO.NotificationHistory;
using Application.Responses;
using MediatR;

namespace Application.CQRS.NotificationHistory.Commands
{
    public class CreateNotificationHistoryCommand : IRequest<BaseCommandResponse>
    {
        public required CreateNotificationHistoryDto CreateNotificationHistoryDto { get; set; }
    }
}