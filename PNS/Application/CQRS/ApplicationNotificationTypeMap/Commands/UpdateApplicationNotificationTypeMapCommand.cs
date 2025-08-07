using MediatR;
using Application.DTO.ApplicationNotificationTypeMap;
using System;

namespace Application.CQRS.ApplicationNotificationTypeMap.Commands
{
    public class UpdateApplicationNotificationTypeMapCommand : IRequest<Unit>
    {
        public required UpdateApplicationNotificationTypeMapDto UpdateApplicationNotificationTypeMapDto { get; set; }
    }
}