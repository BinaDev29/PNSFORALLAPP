using AutoMapper;
using MediatR;
using Application.CQRS.Notification.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notification.Handlers;

public class UpdateNotificationCommandHandler(IGenericRepository<Domain.Models.Notification> repository, IMapper mapper)
    : IRequestHandler<UpdateNotificationCommand, Unit>
{
    public async Task<Unit> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await repository.Get(request.UpdateNotificationDto.Id);

        if (notification is null) // Null check ተስተካክሏል
        {
            throw new NotFoundException(nameof(Domain.Models.Notification), request.UpdateNotificationDto.Id);
        }

        mapper.Map(request.UpdateNotificationDto, notification);

        await repository.Update(notification);

        return Unit.Value;
    }
}