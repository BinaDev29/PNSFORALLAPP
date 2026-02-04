// File Path: Application/CQRS/NotificationHistory/Handlers/GetNotificationHistoriesListQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.NotificationHistory.Queries;
using Application.DTO.NotificationHistory;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationHistory.Handlers
{
    public class GetNotificationHistoriesListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetNotificationHistoriesListQuery, List<NotificationHistoryDto>>
    {
        public async Task<List<NotificationHistoryDto>> Handle(GetNotificationHistoriesListQuery request, CancellationToken cancellationToken)
        {
            var histories = await unitOfWork.NotificationHistories.GetNotificationHistoriesWithDetails(request.UserId, request.IsAdmin, cancellationToken);
            return mapper.Map<List<NotificationHistoryDto>>(histories);
        }
    }
}