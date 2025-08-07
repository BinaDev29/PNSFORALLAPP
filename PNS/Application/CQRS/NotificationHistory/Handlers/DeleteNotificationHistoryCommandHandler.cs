// DeleteNotificationHistoryCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.NotificationHistory.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationHistory.Handlers;

public class DeleteNotificationHistoryCommandHandler(IGenericRepository<Domain.Models.NotificationHistory> repository)
    : IRequestHandler<DeleteNotificationHistoryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteNotificationHistoryCommand request, CancellationToken cancellationToken)
    {
        var notificationHistory = await repository.Get(request.Id);

        if (notificationHistory is null)
        {
            throw new NotFoundException(nameof(Domain.Models.NotificationHistory), request.Id);
        }

        await repository.Delete(notificationHistory);

        return Unit.Value;
    }
}