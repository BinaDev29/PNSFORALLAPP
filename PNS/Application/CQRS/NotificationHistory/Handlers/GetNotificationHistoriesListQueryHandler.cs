// GetNotificationHistoriesListQueryHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.NotificationHistory.Queries;
using Application.Contracts.IRepository;
using Application.DTO.NotificationHistory;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Application.CQRS.NotificationHistory.Handlers;

public class GetNotificationHistoriesListQueryHandler(IGenericRepository<Domain.Models.NotificationHistory> repository, IMapper mapper)
    : IRequestHandler<GetNotificationHistoriesListQuery, IReadOnlyList<NotificationHistoryDto>>
{
    public async Task<IReadOnlyList<NotificationHistoryDto>> Handle(GetNotificationHistoriesListQuery request, CancellationToken cancellationToken)
    {
        var notificationHistories = await repository.GetAll();
        return mapper.Map<IReadOnlyList<NotificationHistoryDto>>(notificationHistories);
    }
}