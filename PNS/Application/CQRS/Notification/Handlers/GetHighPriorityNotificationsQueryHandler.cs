﻿// File Path: Application/CQRS/Notification/Handlers/GetHighPriorityNotificationsQueryHandler.cs
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
    public class GetHighPriorityNotificationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetHighPriorityNotificationsQuery, List<NotificationDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<NotificationDto>> Handle(GetHighPriorityNotificationsQuery request, CancellationToken cancellationToken)
        {
            var highPriorityNotifications = await _unitOfWork.Notifications.Get(
                q => q.ClientApplicationId == request.ClientApplicationId && q.Priority != null && q.Priority.Level == 1, cancellationToken
            );
            return _mapper.Map<List<NotificationDto>>(highPriorityNotifications);
        }
    }
}