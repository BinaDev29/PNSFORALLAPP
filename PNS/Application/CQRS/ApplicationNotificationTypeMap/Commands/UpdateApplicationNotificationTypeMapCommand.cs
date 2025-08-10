// File Path: Application/CQRS/ApplicationNotificationTypeMap/Commands/UpdateApplicationNotificationTypeMapCommand.cs
using Application.DTO.ApplicationNotificationTypeMap;
using MediatR;

namespace Application.CQRS.ApplicationNotificationTypeMap.Commands
{
    public class UpdateApplicationNotificationTypeMapCommand : IRequest<Unit>
    {
        public required UpdateApplicationNotificationTypeMapDto UpdateApplicationNotificationTypeMapDto { get; set; }
    }
}