using Application.Contracts.IRepository;
using Application.CQRS.Notification.Queries;
using Application.DTO.Notification;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notification.Handlers
{
    public class GetNotificationStatisticsQueryHandler : IRequestHandler<GetNotificationStatisticsQuery, NotificationStatisticsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNotificationStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NotificationStatisticsDto> Handle(GetNotificationStatisticsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Notifications.GetStatisticsAsync(
                request.StartDate, 
                request.EndDate, 
                request.ClientApplicationId, 
                cancellationToken);
        }
    }
}
