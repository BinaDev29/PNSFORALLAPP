// GetNotificationHistoryDetailQueryHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.NotificationHistory.Queries;
using Application.Contracts.IRepository;
using Application.DTO.NotificationHistory;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationHistory.Handlers;

public class GetNotificationHistoryDetailQueryHandler(IGenericRepository<Domain.Models.NotificationHistory> repository, IMapper mapper)
    : IRequestHandler<GetNotificationHistoryDetailQuery, NotificationHistoryDto>
{
    public async Task<NotificationHistoryDto> Handle(GetNotificationHistoryDetailQuery request, CancellationToken cancellationToken)
    {
        var notificationHistory = await repository.Get(request.Id);

        if (notificationHistory is null)
        {
            throw new NotFoundException(nameof(Domain.Models.NotificationHistory), request.Id);
        }

        return mapper.Map<NotificationHistoryDto>(notificationHistory);
    }
}