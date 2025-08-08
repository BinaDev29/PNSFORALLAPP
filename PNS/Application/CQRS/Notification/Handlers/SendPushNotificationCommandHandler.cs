// File Path: Application/CQRS/Notification/Handlers/SendPushNotificationCommandHandler.cs
using MediatR;
using Application.CQRS.Notification.Commands;
using Application.Contracts.IRepository;
using Application.Contracts.IServices;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notification.Handlers
{
    public class SendPushNotificationCommandHandler(INotificationRepository repository, IPushNotificationService pushNotificationService)
        : IRequestHandler<SendPushNotificationCommand, Unit>
    {
        public async Task<Unit> Handle(SendPushNotificationCommand request, CancellationToken cancellationToken)
        {
            // notificationን በዳታቤዝ ውስጥ ማስቀመጥ
            request.Notification = await repository.Add(request.Notification, cancellationToken);

            // notificationን ወደ `queue` መላክ
            await pushNotificationService.SendNotificationAsync(request.Notification);

            return Unit.Value;
        }
    }
}