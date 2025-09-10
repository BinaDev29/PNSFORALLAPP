// File Path: Application/CQRS/NotificationType/Handlers/DeleteNotificationTypeCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.NotificationType.Commands;
using Application.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationType.Handlers
{
    public class DeleteNotificationTypeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteNotificationTypeCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteNotificationTypeCommand request, CancellationToken cancellationToken)
        {
            var notificationType = await unitOfWork.NotificationTypes.Get(request.Id);

            if (notificationType == null)
            {
                throw new NotFoundException(nameof(Domain.Models.NotificationType), request.Id);
            }

            await unitOfWork.NotificationTypes.Delete(notificationType);
            await unitOfWork.Save(cancellationToken);

            return Unit.Value;
        }
    }
}