// File Path: Application/CQRS/NotificationType/Handlers/GetNotificationTypesListQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.NotificationType.Queries;
using Application.DTO.NotificationType;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationType.Handlers
{
    public class GetNotificationTypesListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetNotificationTypesListQuery, List<NotificationTypeDto>>
    {
        public async Task<List<NotificationTypeDto>> Handle(GetNotificationTypesListQuery request, CancellationToken cancellationToken)
        {
            var notificationTypes = await unitOfWork.NotificationTypes.GetAll();
            return mapper.Map<List<NotificationTypeDto>>(notificationTypes);
        }
    }
}