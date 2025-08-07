// CreateApplicationNotificationTypeMapCommand.cs
using MediatR;
using Application.DTO.ApplicationNotificationTypeMap;
using Application.Responses;

namespace Application.CQRS.ApplicationNotificationTypeMap.Commands
{
    public class CreateApplicationNotificationTypeMapCommand : IRequest<BaseCommandResponse>
    {
        public required CreateApplicationNotificationTypeMapDto MapDto { get; set; }
    }
}