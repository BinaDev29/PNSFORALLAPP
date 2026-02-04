// File Path: Application/CQRS/Notification/Handlers/GetNotificationsListQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.Notification.Queries;
using Application.DTO.Notification;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notification.Handlers
{
    public class GetNotificationsListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetNotificationsListQuery, List<NotificationDto>>
    {
        public async Task<List<NotificationDto>> Handle(GetNotificationsListQuery request, CancellationToken cancellationToken)
        {
            var notifications = await unitOfWork.Notifications.GetByUserId(request.UserId, request.IsAdmin, cancellationToken);
            return mapper.Map<List<NotificationDto>>(notifications);
        }
    }
}