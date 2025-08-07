using AutoMapper;
using MediatR;
using Application.CQRS.Notification.Queries;
using Application.Contracts.IRepository;
using Application.DTO.Notification;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notification.Handlers;

public class GetNotificationDetailQueryHandler(IGenericRepository<Domain.Models.Notification> repository, IMapper mapper)
    : IRequestHandler<GetNotificationDetailQuery, NotificationDto>
{
    public async Task<NotificationDto> Handle(GetNotificationDetailQuery request, CancellationToken cancellationToken)
    {
        var notification = await repository.Get(request.Id);

        if (notification is null)
        {
            throw new NotFoundException(nameof(Domain.Models.Notification), request.Id);
        }

        return mapper.Map<NotificationDto>(notification);
    }
}