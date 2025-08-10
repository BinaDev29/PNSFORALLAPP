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
    public class GetApplicationNotificationTypeMapsListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetApplicationNotificationTypeMapsListQuery, List<ApplicationNotificationTypeMapDto>>
    {
        public async Task<List<ApplicationNotificationTypeMapDto>> Handle(GetApplicationNotificationTypeMapsListQuery request, CancellationToken cancellationToken)
        {
            var maps = await unitOfWork.ApplicationNotificationTypeMaps.GetAll(cancellationToken);
            return mapper.Map<List<ApplicationNotificationTypeMapDto>>(maps);
        }
    }
}