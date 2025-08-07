using MediatR;
using Application.DTO.ApplicationNotificationTypeMap; // ይህ ትክክለኛው DTO ነው።
using System.Collections.Generic;
using Application.CQRS.ApplicationNotificationTypeMap.Queries;

namespace Application.CQRS.ApplicationNotificationTypeMap.Queries
{
    public class GetApplicationNotificationTypeMapsListQuery : IRequest<IReadOnlyList<ApplicationNotificationTypeMapDto>>
    {
    }
}