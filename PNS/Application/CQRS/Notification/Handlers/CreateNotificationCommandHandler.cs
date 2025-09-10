// File Path: Application/CQRS/Notification/Handlers/CreateNotificationCommandHandler.cs
using Application.Contracts;
using Application.Contracts.IRepository;
using Application.CQRS.Notification.Commands;
using Application.DTO.Notification.Validator;
using Application.Exceptions;
using Application.Models.Email;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.CQRS.Notification.Handlers
{
    public class CreateNotificationCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService, CreateNotificationDtoValidator validator)
        : IRequestHandler<CreateNotificationCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            var validationResult = await validator.ValidateAsync(request.CreateNotificationDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            var clientApplication = await unitOfWork.ClientApplications.Get(request.CreateNotificationDto.ClientApplicationId, cancellationToken);
            if (clientApplication is null)
            {
                throw new NotFoundException(nameof(ClientApplication), request.CreateNotificationDto.ClientApplicationId);
            }

            var notification = mapper.Map<Domain.Models.Notification>(request.CreateNotificationDto);
            notification.Title = $"{clientApplication.Name}";
            notification.ReceivedTime = DateTime.UtcNow;

            await unitOfWork.Notifications.Add(notification, cancellationToken);
            await unitOfWork.Save(cancellationToken);

            // Fix: The email service should handle its own credentials from configuration.
            // The handler's responsibility is to provide the message content.
            var emailMessage = new EnhancedEmailMessage
            {
                // Fix: Properly convert the list of recipients to string
                To = notification.To.Select(o => o.ToString()).ToList(),
                From = clientApplication.SenderEmail,
                Subject = notification.Title,
                BodyHtml = $"<p><h2>{clientApplication.Name}.</h2></p><p><em>{notification.Message}</em></p><img src='{clientApplication.Logo}' alt='Client Logo' style='width:100px; height:auto; border:2px solid black;border-radius:10px;padding:20px; box-shadow:0px 4px 8px rgba(0,0,0,0.8);background-color:white;margin-top:10px' />",
            };

            var emailResult = await emailService.SendEmail(emailMessage);

            // Fix: Your NotificationHistory class is a namespace. This will cause an error.
            // You need to rename the class to avoid a naming conflict.
            // Example: var notificationHistory = new NotificationHistoryRecord();
            var notificationHistory = new Domain.Models.NotificationHistory
            {
                Id = Guid.NewGuid(),
                NotificationId = notification.Id,
                SentDate = notification.ReceivedTime.Value,
                Status = emailResult ? "Sent" : "Failed",
            };

            await unitOfWork.NotificationHistories.Add(notificationHistory, cancellationToken);
            await unitOfWork.Save(cancellationToken);

            response.Success = true;
            response.Message = emailResult ? "Creation Successful and Emails Sent" : "Creation Successful, but Emails Failed";
            response.Id = notification.Id;

            return response;
        }
    }
}