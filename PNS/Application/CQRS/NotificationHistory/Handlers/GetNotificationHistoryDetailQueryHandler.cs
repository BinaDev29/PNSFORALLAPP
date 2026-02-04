// File Path: Application/CQRS/NotificationHistory/Handlers/GetNotificationHistoryDetailQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.NotificationHistory.Queries;
using Application.DTO.NotificationHistory;
using Application.Exceptions;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationHistory.Handlers
{
    public class GetNotificationHistoryDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetNotificationHistoryDetailQuery, NotificationHistoryDto>
    {
        public async Task<NotificationHistoryDto> Handle(GetNotificationHistoryDetailQuery request, CancellationToken cancellationToken)
        {
            var history = await unitOfWork.NotificationHistories.GetNotificationHistoryWithDetails(request.Id, cancellationToken);

            if (history is null)
            {
                throw new NotFoundException(nameof(Domain.Models.NotificationHistory), request.Id);
            }

            return mapper.Map<NotificationHistoryDto>(history);
        }
    }
}