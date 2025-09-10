// File Path: Application/CQRS/NotificationType/Handlers/UpdateNotificationTypeCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.NotificationType.Commands;
using Application.DTO.NotificationType.Validator;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using System;
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

            var notificationType = await unitOfWork.NotificationTypes.Get(request.UpdateNotificationTypeDto.Id);

            if (notificationType == null)
            {
                throw new NotFoundException(nameof(Domain.Models.NotificationType), request.UpdateNotificationTypeDto.Id);
            }

            mapper.Map(request.UpdateNotificationTypeDto, notificationType);

            await unitOfWork.NotificationTypes.Update(notificationType);
            await unitOfWork.Save(cancellationToken);

            return Unit.Value;
        }
    }
}