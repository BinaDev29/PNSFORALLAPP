// File Path: Application/CQRS/Notification/Handlers/GetNotificationDetailQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.Notification.Queries;
using Application.DTO.Notification;
using Application.Exceptions;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notification.Handlers
{
    public class GetNotificationDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetNotificationDetailQuery, NotificationDto>
    {
        public async Task<NotificationDto> Handle(GetNotificationDetailQuery request, CancellationToken cancellationToken)
        {
            var notification = await unitOfWork.Notifications.Get(request.Id, cancellationToken);

            if (notification is null)
            {
                throw new NotFoundException(nameof(Notification), request.Id);
            }

            return mapper.Map<NotificationDto>(notification);
        }
    }
}