// File Path: Application/CQRS/NotificationType/Handlers/GetNotificationTypeDetailQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.NotificationType.Queries;
using Application.DTO.NotificationType;
using Application.Exceptions;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationType.Handlers
{
    public class GetNotificationTypeDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetNotificationTypeDetailQuery, NotificationTypeDto>
    {
        public async Task<NotificationTypeDto> Handle(GetNotificationTypeDetailQuery request, CancellationToken cancellationToken)
        {
            var type = await unitOfWork.NotificationTypes.Get(request.Id, cancellationToken);

            if (type is null)
            {
                throw new NotFoundException(nameof(Domain.Models.NotificationType), request.Id);
            }

            return mapper.Map<NotificationTypeDto>(type);
        }
    }
}