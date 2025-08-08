// File Path: Application/CQRS/ApplicationNotificationTypeMap/Queries/GetApplicationNotificationTypeMapDetailQueryHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.ApplicationNotificationTypeMap.Queries;
using Application.Contracts.IRepository;
using Application.DTO.ApplicationNotificationTypeMap;
using Application.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ApplicationNotificationTypeMap.Handlers
{
    public class GetApplicationNotificationTypeMapDetailQueryHandler(IApplicationNotificationTypeMapRepository repository, IMapper mapper)
        : IRequestHandler<GetApplicationNotificationTypeMapDetailQuery, ApplicationNotificationTypeMapDto>
    {
        public async Task<ApplicationNotificationTypeMapDto> Handle(GetApplicationNotificationTypeMapDetailQuery request, CancellationToken cancellationToken)
        {
            var map = await repository.Get(request.ClientApplicationId, request.NotificationTypeId, cancellationToken);

            if (map == null)
            {
                throw new NotFoundException(nameof(ApplicationNotificationTypeMap), $"{request.ClientApplicationId}, {request.NotificationTypeId}");
            }

            return mapper.Map<ApplicationNotificationTypeMapDto>(map);
        }
    }
}