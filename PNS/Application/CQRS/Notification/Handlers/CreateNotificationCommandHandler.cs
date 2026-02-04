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
        private readonly IDashboardHubService _hubService;

        public CreateNotificationCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            CreateNotificationDtoValidator validator,
            ILogger<CreateNotificationCommandHandler> logger,
            IDashboardHubService hubService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _validator = validator;
            _logger = logger;
            _hubService = hubService;
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

                // Ownership check: If not admin, the client application must belong to the user
                if (!request.IsAdmin && clientApplication.CreatedBy != request.UserId)
                {
                    response.Success = false;
                    response.Message = "You do not have permission to use this client application";
                    response.Errors = new List<string> { "Unauthorized access to client application" };
                    return response;
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

                // MODIFIED: Send individual emails to each recipient to ensure privacy
                var recipients = notification.To
                    .Select(o => o?.ToString())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Cast<string>()
                    .ToList();
                var successfulSends = 0;
                var failedRecipients = new List<string>();

                _logger.LogInformation("Sending individual notification emails to {RecipientCount} recipients to ensure privacy", recipients.Count);

                foreach (var recipient in recipients)
                {
                    try
                    {
                        // Create individual email message for each recipient
                        var individualEmailMessage = new EnhancedEmailMessage
                        {
                            To = new List<string> { recipient }, // Only this recipient
                            From = clientApplication.SenderEmail,
                            Subject = notification.Title,
                            BodyHtml = $"<p><h2>{clientApplication.Name}.</h2></p><p><em>{notification.Message}</em></p><img src='{clientApplication.Logo}' alt='Client Logo' style='width:100px; height:auto; border:2px solid black;border-radius:10px;padding:20px; box-shadow:0px 4px 8px rgba(0,0,0,0.8);background-color:white;margin-top:10px' />",
                            TrackingId = $"{notification.Id}_{recipient}",
                            EnableTracking = true,
                            // Pass client application credentials to SMTP provider
                            Metadata = new Dictionary<string, object>
                            {
                                { "SenderEmail", clientApplication.SenderEmail },
                                { "AppPassword", clientApplication.AppPassword },
                                { "ClientApplicationId", clientApplication.Id.ToString() },
                                { "RecipientEmail", recipient }
                            }
                        };

                        _logger.LogDebug("Sending individual notification email to {Recipient} using client SMTP credentials: {SenderEmail}", recipient, clientApplication.SenderEmail);

                        var emailResult = await _emailService.SendEmail(individualEmailMessage);

                        if (emailResult)
                        {
                            successfulSends++;
                            _logger.LogDebug("Successfully sent notification email to {Recipient}", recipient);
                        }
                        else
                        {
                            failedRecipients.Add(recipient);
                            _logger.LogWarning("Failed to send notification email to {Recipient}", recipient);
                        }

                        // Create individual notification history for each recipient
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
                    }
                    catch (Exception ex)
                    {
                        failedRecipients.Add(recipient);
                        _logger.LogError(ex, "Error sending notification email to {Recipient}", recipient);
                    }
                }

                await _unitOfWork.Save(cancellationToken);

                // Prepare response based on results
                var allSuccessful = successfulSends == recipients.Count;
                var partialSuccess = successfulSends > 0 && successfulSends < recipients.Count;

                response.Success = allSuccessful || partialSuccess;

                if (allSuccessful)
                {
                    response.Message = $"Notification created and individual emails sent successfully to all {successfulSends} recipients using client SMTP credentials";
                }
                else if (partialSuccess)
                {
                    response.Message = $"Notification created and individual emails sent successfully to {successfulSends}/{recipients.Count} recipients. Failed recipients: {string.Join(", ", failedRecipients)}";
                    response.Errors = new List<string> { $"Failed to send to: {string.Join(", ", failedRecipients)}" };
                }
                else
                {
                    response.Message = $"Notification created, but all individual emails failed to send. Failed recipients: {string.Join(", ", failedRecipients)}";
                    response.Errors = new List<string> { $"Failed to send to all recipients: {string.Join(", ", failedRecipients)}" };
                }

                response.Id = notification.Id;

                _logger.LogInformation("Notification {NotificationId} processed. Individual emails sent: {SuccessCount}/{TotalCount}",
                    notification.Id, successfulSends, recipients.Count);

                // Send real-time update to dashboard
                await _hubService.SendNotificationUpdate($"New notification processed for {clientApplication.Name}. Success: {successfulSends}/{recipients.Count}");
                await _hubService.SendDashboardStats(new { 
                    Total = await _unitOfWork.Notifications.Count(cancellationToken),
                    RecentSuccess = successfulSends 
                });
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