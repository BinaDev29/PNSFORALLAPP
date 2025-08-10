// File Path: Application/CQRS/ApplicationNotificationTypeMap/Queries/GetApplicationNotificationTypeMapsListQuery.cs
using Application.DTO.ApplicationNotificationTypeMap;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.ApplicationNotificationTypeMap.Queries
{
    public class GetApplicationNotificationTypeMapsListQuery : IRequest<List<ApplicationNotificationTypeMapDto>>
    {
    }
}