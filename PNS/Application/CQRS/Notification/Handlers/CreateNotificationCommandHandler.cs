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
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.CQRS.Notification.Handlers
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, BaseCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly CreateNotificationDtoValidator _validator;
        private readonly ILogger<CreateNotificationCommandHandler> _logger;

        public CreateNotificationCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            CreateNotificationDtoValidator validator,
            ILogger<CreateNotificationCommandHandler> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _validator = validator;
            _logger = logger;
        }

        public async Task<BaseCommandResponse> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validationResult = await _validator.ValidateAsync(request.CreateNotificationDto, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult);
                }

                var clientApplication = await _unitOfWork.ClientApplications.Get(request.CreateNotificationDto.ClientApplicationId, cancellationToken);
                if (clientApplication is null)
                {
                    throw new NotFoundException(nameof(ClientApplication), request.CreateNotificationDto.ClientApplicationId);
                }

                // Validate that client application has required email credentials
                if (string.IsNullOrEmpty(clientApplication.SenderEmail))
                {
                    response.Success = false;
                    response.Message = "Client application must have SenderEmail configured";
                    response.Errors = new List<string> { "SenderEmail is required for the client application" };
                    return response;
                }

                if (string.IsNullOrEmpty(clientApplication.AppPassword))
                {
                    response.Success = false;
                    response.Message = "Client application must have AppPassword configured";
                    response.Errors = new List<string> { "AppPassword is required for the client application" };
                    return response;
                }

                var notification = _mapper.Map<Domain.Models.Notification>(request.CreateNotificationDto);
                notification.Title = $"{clientApplication.Name}";
                notification.ReceivedTime = DateTime.UtcNow;

                await _unitOfWork.Notifications.Add(notification, cancellationToken);
                await _unitOfWork.Save(cancellationToken);

                // Create email message using client application's SMTP credentials
                var emailMessage = new EnhancedEmailMessage
                {
                    To = notification.To.Select(o => o.ToString()).ToList(),
                    From = clientApplication.SenderEmail, // Use client application's sender email
                    Subject = notification.Title,
                    BodyHtml = $"<p><h2>{clientApplication.Name}.</h2></p><p><em>{notification.Message}</em></p><img src='{clientApplication.Logo}' alt='Client Logo' style='width:100px; height:auto; border:2px solid black;border-radius:10px;padding:20px; box-shadow:0px 4px 8px rgba(0,0,0,0.8);background-color:white;margin-top:10px' />",
                    TrackingId = notification.Id.ToString(),
                    EnableTracking = true,
                    // Pass client application credentials to SMTP provider
                    Metadata = new Dictionary<string, object>
                    {
                        { "SenderEmail", clientApplication.SenderEmail },
                        { "AppPassword", clientApplication.AppPassword },
                        { "ClientApplicationId", clientApplication.Id.ToString() }
                    }
                };

                _logger.LogInformation("Sending notification email using client SMTP credentials: {SenderEmail}", clientApplication.SenderEmail);

                var emailResult = await _emailService.SendEmail(emailMessage);

                // Create notification history
                var notificationHistory = new Domain.Models.NotificationHistory
                {
                    Id = Guid.NewGuid(),
                    NotificationId = notification.Id,
                    SentDate = notification.ReceivedTime.Value,
                    Status = emailResult ? "Sent" : "Failed",
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _unitOfWork.NotificationHistories.Add(notificationHistory, cancellationToken);
                await _unitOfWork.Save(cancellationToken);

                response.Success = true;
                response.Message = emailResult ? "Notification created and email sent successfully using client SMTP credentials" : "Notification created, but email failed to send";
                response.Id = notification.Id;

                _logger.LogInformation("Notification {NotificationId} processed. Email result: {EmailResult}", notification.Id, emailResult);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error processing notification: {ex.Message}";
                response.Errors = new List<string> { ex.Message };
                _logger.LogError(ex, "Error processing notification");
            }

            return response;
        }
    }
}