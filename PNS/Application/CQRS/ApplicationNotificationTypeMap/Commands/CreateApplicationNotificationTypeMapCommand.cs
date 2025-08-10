// File Path: Application/CQRS/ApplicationNotificationTypeMap/Commands/CreateApplicationNotificationTypeMapCommand.cs
using Application.DTO.ApplicationNotificationTypeMap;
using Application.Responses;
using MediatR;

namespace Application.CQRS.ApplicationNotificationTypeMap.Commands
{
    public class CreateApplicationNotificationTypeMapCommand : IRequest<BaseCommandResponse>
    {
        public required CreateApplicationNotificationTypeMapDto CreateApplicationNotificationTypeMapDto { get; set; }
    }
}