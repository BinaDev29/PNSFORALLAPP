// File Path: Application/CQRS/Notification/Handlers/MarkNotificationAsSeenCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.Notification.Commands;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notification.Handlers
{
    public class MarkNotificationAsSeenCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<MarkNotificationAsSeenCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(MarkNotificationAsSeenCommand request, CancellationToken cancellationToken)
        {
            var notification = await _unitOfWork.Notifications.Get(request.Id, cancellationToken);
            
            if (notification == null)
            {
                return Unit.Value;
            }

            notification.SeenTime = DateTime.UtcNow;
            await _unitOfWork.Notifications.Update(notification, cancellationToken);
            await _unitOfWork.Save(cancellationToken);

            return Unit.Value;
        }
    }
}