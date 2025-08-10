// File Path: Application/CQRS/NotificationType/Queries/GetNotificationTypesListQuery.cs
using Application.DTO.NotificationType;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.NotificationType.Queries
{
    public class GetNotificationTypesListQuery : IRequest<List<NotificationTypeDto>>
    {
    }
}