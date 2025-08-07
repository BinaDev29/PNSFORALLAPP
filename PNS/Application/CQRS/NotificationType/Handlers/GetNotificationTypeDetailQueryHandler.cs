using AutoMapper;
using MediatR;
using Application.CQRS.NotificationType.Queries;
using Application.Contracts.IRepository;
using Application.DTO.NotificationType;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationType.Handlers;

public class GetNotificationTypeDetailQueryHandler(IGenericRepository<Domain.Models.NotificationType> repository, IMapper mapper)
    : IRequestHandler<GetNotificationTypeDetailQuery, NotificationTypeDto>
{
    public async Task<NotificationTypeDto> Handle(GetNotificationTypeDetailQuery request, CancellationToken cancellationToken)
    {
        var notificationType = await repository.Get(request.Id);

        if (notificationType is null)
        {
            throw new NotFoundException(nameof(Domain.Models.NotificationType), request.Id);
        }

        return mapper.Map<NotificationTypeDto>(notificationType);
    }
}