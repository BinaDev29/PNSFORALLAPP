using AutoMapper;
using MediatR;
using Application.CQRS.Notification.Queries;
using Application.Contracts.IRepository;
using Application.DTO.Notification;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Application.CQRS.Notification.Handlers;

public class GetNotificationsListQueryHandler(IGenericRepository<Domain.Models.Notification> repository, IMapper mapper)
    : IRequestHandler<GetNotificationsListQuery, IReadOnlyList<NotificationDto>>
{
    public async Task<IReadOnlyList<NotificationDto>> Handle(GetNotificationsListQuery request, CancellationToken cancellationToken)
    {
        var notifications = await repository.GetAll();
        return mapper.Map<IReadOnlyList<NotificationDto>>(notifications);
    }
}