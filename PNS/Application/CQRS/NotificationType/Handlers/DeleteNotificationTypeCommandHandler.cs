using AutoMapper;
using MediatR;
using Application.CQRS.NotificationType.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationType.Handlers;

public class DeleteNotificationTypeCommandHandler(IGenericRepository<Domain.Models.NotificationType> repository)
    : IRequestHandler<DeleteNotificationTypeCommand, Unit>
{
    public async Task<Unit> Handle(DeleteNotificationTypeCommand request, CancellationToken cancellationToken)
    {
        var notificationType = await repository.Get(request.Id);

        if (notificationType is null)
        {
            throw new NotFoundException(nameof(Domain.Models.NotificationType), request.Id);
        }

        await repository.Delete(notificationType);

        return Unit.Value;
    }
}