// File Path: Application/CQRS/Notification/Handlers/UpdateNotificationCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.Notification.Commands;
using Application.DTO.Notification.Validator;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notification.Handlers
{
    // Inject the validator as a dependency
    public class UpdateNotificationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, UpdateNotificationDtoValidator validator)
        : IRequestHandler<UpdateNotificationCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            // Use the injected validator instance
            var validationResult = await validator.ValidateAsync(request.UpdateNotificationDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            var notification = await unitOfWork.Notifications.Get(request.UpdateNotificationDto.Id, cancellationToken);

            if (notification is null)
            {
                throw new NotFoundException(nameof(Domain.Models.Notification), request.UpdateNotificationDto.Id);
            }

            mapper.Map(request.UpdateNotificationDto, notification);
            await unitOfWork.Notifications.Update(notification, cancellationToken);

            // Critical fix: Add the save call
            await unitOfWork.Save(cancellationToken);

            return Unit.Value;
        }
    }
}