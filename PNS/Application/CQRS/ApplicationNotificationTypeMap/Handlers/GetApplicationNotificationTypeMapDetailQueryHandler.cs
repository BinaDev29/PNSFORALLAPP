// File Path: Application/CQRS/ApplicationNotificationTypeMap/Handlers/GetApplicationNotificationTypeMapDetailQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ApplicationNotificationTypeMap.Queries;
using Application.DTO.ApplicationNotificationTypeMap;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ApplicationNotificationTypeMap.Handlers
{
    public class GetApplicationNotificationTypeMapDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetApplicationNotificationTypeMapDetailQuery, ApplicationNotificationTypeMapDto>
    {
        public async Task<ApplicationNotificationTypeMapDto> Handle(GetApplicationNotificationTypeMapDetailQuery request, CancellationToken cancellationToken)
        {
            var map = await unitOfWork.ApplicationNotificationTypeMaps.GetByKeys(request.ClientApplicationId, request.NotificationTypeId, cancellationToken);

            if (map is null)
            {
                throw new NotFoundException($"{nameof(ApplicationNotificationTypeMap)} with ClientApplicationId: {request.ClientApplicationId} and NotificationTypeId: {request.NotificationTypeId}", "not found");
            }

            return mapper.Map<ApplicationNotificationTypeMapDto>(map);
        }
    }
}