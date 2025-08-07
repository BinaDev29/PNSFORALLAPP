using AutoMapper;
using MediatR;
using Application.CQRS.NotificationType.Queries;
using Application.Contracts.IRepository;
using Application.DTO.NotificationType;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Application.CQRS.NotificationType.Handlers;

public class GetNotificationTypesListQueryHandler(IGenericRepository<Domain.Models.NotificationType> repository, IMapper mapper)
    : IRequestHandler<GetNotificationTypesListQuery, IReadOnlyList<NotificationTypeDto>>
{
    public async Task<IReadOnlyList<NotificationTypeDto>> Handle(GetNotificationTypesListQuery request, CancellationToken cancellationToken)
    {
        var notificationTypes = await repository.GetAll();
        return mapper.Map<IReadOnlyList<NotificationTypeDto>>(notificationTypes);
    }
}