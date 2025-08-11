// File Path: Application/CQRS/Notification/Handlers/GetSeenNotificationsQueryHandler.cs
// ይህ ኮድ አሁን ከላይ ባለው የRepository ማስተካከያ መሰረት በትክክል ይሰራል
using Application.Contracts.IRepository;
using Application.CQRS.Notification.Queries;
using Application.DTO.Notification;
using AutoMapper;
using MediatR;

public class GetSeenNotificationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetSeenNotificationsQuery, List<NotificationDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<List<NotificationDto>> Handle(GetSeenNotificationsQuery request, CancellationToken cancellationToken)
    {
        var seenNotifications = await _unitOfWork.Notifications.Get(
            q => q.ClientApplicationId == request.ClientApplicationId && q.SeenTime != null, cancellationToken
        );
        return _mapper.Map<List<NotificationDto>>(seenNotifications);
    }
}