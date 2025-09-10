// File Path: Application/CQRS/Notification/Handlers/DeleteNotificationCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.Notification.Commands;
using Application.Exceptions;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notification.Handlers
{
    public class DeleteNotificationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteNotificationCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await unitOfWork.Notifications.Get(request.Id, cancellationToken);

            if (notification is null)
            {
                throw new NotFoundException(nameof(Notification), request.Id);
            }

            await unitOfWork.Notifications.Delete(notification, cancellationToken);

            // Critical fix: Add the save call
            await unitOfWork.Save(cancellationToken);


            return Unit.Value;
        }
    }
}