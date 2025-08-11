// File Path: Application/CQRS/Notification/Handlers/CreateNotificationCommandHandler.cs
using Application.Contracts;
using Application.Contracts.IRepository;
using Application.CQRS.Notification.Commands;
using Application.DTO.Notification;
using Application.DTO.Notification.Validator;
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
    public class CreateNotificationCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService) : IRequestHandler<CreateNotificationCommand, BaseCommandResponse>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IEmailService _emailService = emailService;

        public async Task<BaseCommandResponse> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateNotificationDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateNotificationDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var clientApplication = await _unitOfWork.ClientApplications.Get(request.CreateNotificationDto.ClientApplicationId, cancellationToken);
            if (clientApplication == null)
            {
                response.Success = false;
                response.Message = "Creation Failed: Client application not found.";
                return response;
            }

            var notification = _mapper.Map<Domain.Models.Notification>(request.CreateNotificationDto);
            notification.Title = $"New Notification from {clientApplication.Name}";
            notification.ReceivedTime = DateTime.UtcNow;

            await _unitOfWork.Notifications.Add(notification, cancellationToken);
            await _unitOfWork.Save(cancellationToken);

            var decryptedAppPassword = EncryptionService.Decrypt(clientApplication.AppPassword);

            bool allEmailsSentSuccessfully = true;
            foreach (var recipient in notification.To)
            {
                var emailMessage = new EmailMessage
                {
                    To = new List<string> { recipient },
                    From = clientApplication.SenderEmail,
                    Subject = notification.Title,
                    BodyHtml = $"<p>New Message from {clientApplication.Name}.</p><p>{notification.Message}</p><img src='{clientApplication.Logo}' alt='Client Logo' style='width:100px; height:auto;' />",
                };

                var emailResult = await _emailService.SendEmail(emailMessage, notification.Id, clientApplication.SenderEmail, decryptedAppPassword);
                if (!emailResult)
                {
                    allEmailsSentSuccessfully = false;
                }
            }

            if (notification.ReceivedTime.HasValue)
            {
                var notificationHistory = new Domain.Models.NotificationHistory
                {
                    Id = Guid.NewGuid(),
                    NotificationId = notification.Id,
                    SentDate = notification.ReceivedTime.Value,
                    Status = allEmailsSentSuccessfully ? "Sent" : "Failed",
                };
                await _unitOfWork.NotificationHistory.Add(notificationHistory, cancellationToken);
                await _unitOfWork.Save(cancellationToken);
            }

            response.Success = true;
            response.Message = allEmailsSentSuccessfully ? "Creation Successful and Emails Sent" : "Creation Successful, but some Emails Failed";
            response.Id = notification.Id;

            return response;
        }
    }
}