// UpdateNotificationHistoryCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.NotificationHistory.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationHistory.Handlers;

public class UpdateNotificationHistoryCommandHandler(IGenericRepository<Domain.Models.NotificationHistory> repository, IMapper mapper)
    : IRequestHandler<UpdateNotificationHistoryCommand, Unit>
{
    public async Task<Unit> Handle(UpdateNotificationHistoryCommand request, CancellationToken cancellationToken)
    {
        var notificationHistory = await repository.Get(request.UpdateNotificationHistoryDto.Id);

        if (notificationHistory is null)
        {
            throw new NotFoundException(nameof(Domain.Models.NotificationHistory), request.UpdateNotificationHistoryDto.Id);
        }

        mapper.Map(request.UpdateNotificationHistoryDto, notificationHistory);

        await repository.Update(notificationHistory);

        return Unit.Value;
    }
}