// File Path: Application/CQRS/NotificationType/Handlers/UpdateNotificationTypeCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.NotificationType.Commands;
using Application.DTO.NotificationType.Validator;
using Application.Exceptions;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationType.Handlers
{
    public class UpdateNotificationTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateNotificationTypeCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateNotificationTypeCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateNotificationTypeDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UpdateNotificationTypeDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            var notificationType = await unitOfWork.NotificationTypes.Get(request.UpdateNotificationTypeDto.Id, cancellationToken);

            if (notificationType is null)
            {
                throw new NotFoundException(nameof(Domain.Models.NotificationType), request.UpdateNotificationTypeDto.Id);
            }

            mapper.Map(request.UpdateNotificationTypeDto, notificationType);
            await unitOfWork.NotificationTypes.Update(notificationType, cancellationToken);

            return Unit.Value;
        }
    }
}