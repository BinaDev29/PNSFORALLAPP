// GetApplicationNotificationTypeMapDetailQuery.cs
using MediatR;
using Application.DTO.ApplicationNotificationTypeMap;
using System;

namespace Application.CQRS.ApplicationNotificationTypeMap.Queries
{
    public class GetApplicationNotificationTypeMapDetailQuery : IRequest<ApplicationNotificationTypeMapDto>
    {
        public Guid ClientApplicationId { get; set; }
        public Guid NotificationTypeId { get; set; }
    }
}