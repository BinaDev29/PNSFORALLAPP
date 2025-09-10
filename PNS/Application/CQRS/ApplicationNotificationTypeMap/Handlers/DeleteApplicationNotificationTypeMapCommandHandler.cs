// File Path: Application/CQRS/ApplicationNotificationTypeMap/Handlers/DeleteApplicationNotificationTypeMapCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ApplicationNotificationTypeMap.Commands;
using Application.Exceptions;
using MediatR;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ApplicationNotificationTypeMap.Handlers
{
    public class DeleteApplicationNotificationTypeMapCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteApplicationNotificationTypeMapCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteApplicationNotificationTypeMapCommand request, CancellationToken cancellationToken)
        {
            var map = await unitOfWork.ApplicationNotificationTypeMaps.GetByKeys(request.ClientApplicationId, request.NotificationTypeId, cancellationToken);

            if (map is null)
            {
                // A more consistent exception message
                throw new NotFoundException(nameof(ApplicationNotificationTypeMap), new { request.ClientApplicationId, request.NotificationTypeId });
            }

            await unitOfWork.ApplicationNotificationTypeMaps.Delete(map, cancellationToken);

            // The essential Save() call to commit the deletion
            await unitOfWork.Save(cancellationToken);

            return Unit.Value;
        }
    }
}