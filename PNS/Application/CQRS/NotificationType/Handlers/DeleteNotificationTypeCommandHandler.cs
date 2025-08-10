// File Path: Application/CQRS/NotificationType/Handlers/DeleteNotificationTypeCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.NotificationType.Commands;
using Application.Exceptions;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationType.Handlers
{
    public class DeleteNotificationTypeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteNotificationTypeCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteNotificationTypeCommand request, CancellationToken cancellationToken)
        {
            var notificationType = await unitOfWork.NotificationTypes.Get(request.Id, cancellationToken);

            if (notificationType is null)
            {
                throw new NotFoundException(nameof(Domain.Models.NotificationType), request.Id);
            }

            await unitOfWork.NotificationTypes.Delete(notificationType, cancellationToken);
            return Unit.Value;
        }
    }
}