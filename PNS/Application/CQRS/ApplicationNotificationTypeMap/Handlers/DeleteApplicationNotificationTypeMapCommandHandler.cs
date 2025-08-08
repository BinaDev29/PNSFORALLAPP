// File Path: Application/CQRS/ApplicationNotificationTypeMap/Handlers/DeleteApplicationNotificationTypeMapCommandHandler.cs
using MediatR;
using Application.CQRS.ApplicationNotificationTypeMap.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ApplicationNotificationTypeMap.Handlers;

public class DeleteApplicationNotificationTypeMapCommandHandler(IApplicationNotificationTypeMapRepository repository)
    : IRequestHandler<DeleteApplicationNotificationTypeMapCommand, Unit>
{
    public async Task<Unit> Handle(DeleteApplicationNotificationTypeMapCommand request, CancellationToken cancellationToken)
    {
        var map = await repository.Get(request.ClientApplicationId, request.NotificationTypeId, cancellationToken);

        // 👉 Null ፍተሻው እዚህ ላይ ነው ያለው። መዝገቡ ከሌለ ስህተት ይጥላል።
        if (map is null)
        {
            throw new NotFoundException(nameof(ApplicationNotificationTypeMap), $"{request.ClientApplicationId}, {request.NotificationTypeId}");
        }

        await repository.Delete(map, cancellationToken);

        return Unit.Value;
    }
}