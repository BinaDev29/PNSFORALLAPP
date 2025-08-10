// File Path: Application/CQRS/ApplicationNotificationTypeMap/Queries/GetApplicationNotificationTypeMapDetailQuery.cs
using Application.DTO.ApplicationNotificationTypeMap;
using MediatR;
using System;

namespace Application.CQRS.ApplicationNotificationTypeMap.Queries
{
    public class GetApplicationNotificationTypeMapDetailQuery : IRequest<ApplicationNotificationTypeMapDto>
    {
        public Guid ClientApplicationId { get; set; }
        public Guid NotificationTypeId { get; set; }
    }
}