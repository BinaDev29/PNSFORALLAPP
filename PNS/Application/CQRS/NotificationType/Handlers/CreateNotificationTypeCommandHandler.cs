// File Path: Application/CQRS/NotificationType/Handlers/CreateNotificationTypeCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.NotificationType.Commands;
using Application.DTO.NotificationType.Validator;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationType.Handlers
{
    public class CreateNotificationTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateNotificationTypeCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateNotificationTypeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateNotificationTypeDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateNotificationTypeDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var notificationType = mapper.Map<Domain.Models.NotificationType>(request.CreateNotificationTypeDto);

            // This is the crucial line: it generates a new, unique Guid for the Id.
            notificationType.Id = Guid.NewGuid();

            await unitOfWork.NotificationTypes.Add(notificationType, cancellationToken);
            await unitOfWork.Save(cancellationToken);

            response.Success = true;
            response.Message = "Creation Successful";
            response.Id = notificationType.Id;
            return response;
        }
    }
}