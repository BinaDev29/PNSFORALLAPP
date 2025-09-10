// File Path: Application/CQRS/NotificationType/Handlers/GetNotificationTypeDetailQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.NotificationType.Queries;
using Application.DTO.NotificationType;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationType.Handlers
{
    public class GetNotificationTypeDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetNotificationTypeDetailQuery, NotificationTypeDto>
    {
        public async Task<NotificationTypeDto> Handle(GetNotificationTypeDetailQuery request, CancellationToken cancellationToken)
        {
            var notificationType = await unitOfWork.NotificationTypes.Get(request.Id);
            return mapper.Map<NotificationTypeDto>(notificationType);
        }
    }
}