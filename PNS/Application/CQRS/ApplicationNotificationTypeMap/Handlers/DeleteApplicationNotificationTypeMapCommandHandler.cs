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
                throw new NotFoundException($"{nameof(ApplicationNotificationTypeMap)} with ClientApplicationId: {request.ClientApplicationId} and NotificationTypeId: {request.NotificationTypeId}", "not found");
            }

            await unitOfWork.ApplicationNotificationTypeMaps.Delete(map, cancellationToken);
            return Unit.Value;
        }
    }
}