using AutoMapper;
using MediatR;
using Application.CQRS.Notification.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notification.Handlers;

public class DeleteNotificationCommandHandler(IGenericRepository<Domain.Models.Notification> repository)
    : IRequestHandler<DeleteNotificationCommand, Unit>
{
    public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await repository.Get(request.Id);

        if (notification is null)
        {
            throw new NotFoundException(nameof(Domain.Models.Notification), request.Id);
        }

        await repository.Delete(notification);

        return Unit.Value;
    }
}