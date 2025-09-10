// File Path: Application/CQRS/Notification/Handlers/GetUnseenNotificationsQueryHandler.cs
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
    public class GetUnseenNotificationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUnseenNotificationsQuery, List<NotificationDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<NotificationDto>> Handle(GetUnseenNotificationsQuery request, CancellationToken cancellationToken)
        {
            var unseenNotifications = await _unitOfWork.Notifications.GetWhere(
                q => q.ClientApplicationId == request.ClientApplicationId && q.SeenTime == null,
                cancellationToken
            );
            return _mapper.Map<List<NotificationDto>>(unseenNotifications);
        }
    }
}