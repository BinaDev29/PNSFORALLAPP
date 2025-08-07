using AutoMapper;
using MediatR;
using Application.CQRS.NotificationType.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationType.Handlers;

public class UpdateNotificationTypeCommandHandler(IGenericRepository<Domain.Models.NotificationType> repository, IMapper mapper)
    : IRequestHandler<UpdateNotificationTypeCommand, Unit>
{
    public async Task<Unit> Handle(UpdateNotificationTypeCommand request, CancellationToken cancellationToken)
    {
        var notificationType = await repository.Get(request.UpdateNotificationTypeDto.Id);

        if (notificationType is null) // Null check ተስተካክሏል
        {
            throw new NotFoundException(nameof(Domain.Models.NotificationType), request.UpdateNotificationTypeDto.Id);
        }

        mapper.Map(request.UpdateNotificationTypeDto, notificationType);

        await repository.Update(notificationType);

        return Unit.Value;
    }
}