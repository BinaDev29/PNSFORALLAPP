// File Path: Application/CQRS/ApplicationNotificationTypeMap/Handlers/GetApplicationNotificationTypeMapsListQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ApplicationNotificationTypeMap.Queries;
using Application.DTO.ApplicationNotificationTypeMap;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ApplicationNotificationTypeMap.Handlers
{
    public class GetApplicationNotificationTypeMapsListQueryHandler(IApplicationNotificationTypeMapRepository repository, IMapper mapper)
        : IRequestHandler<GetApplicationNotificationTypeMapsListQuery, IReadOnlyList<ApplicationNotificationTypeMapDto>>
    {
        public async Task<IReadOnlyList<ApplicationNotificationTypeMapDto>> Handle(GetApplicationNotificationTypeMapsListQuery request, CancellationToken cancellationToken)
        {
            var maps = await repository.GetAll(cancellationToken);
            return mapper.Map<IReadOnlyList<ApplicationNotificationTypeMapDto>>(maps);
        }
    }
}