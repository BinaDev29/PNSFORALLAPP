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
using Application.Common.Interfaces;
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
        private readonly IEmailQueueService _emailQueueService;
        private readonly ISmsQueueService _smsQueueService;
        private readonly CreateNotificationDtoValidator _validator;
        private readonly ILogger<CreateNotificationCommandHandler> _logger;
        private readonly IDashboardHubService _hubService;

        public CreateNotificationCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IEmailQueueService emailQueueService,
            ISmsQueueService smsQueueService,
            CreateNotificationDtoValidator validator,
            ILogger<CreateNotificationCommandHandler> logger,
            IDashboardHubService hubService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailQueueService = emailQueueService;
            _smsQueueService = smsQueueService;
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

                // Fetch Notification Type to check if it's Email or SMS
                var notificationType = await _unitOfWork.NotificationTypes.Get(notification.NotificationTypeId);
                var mode = notificationType?.Name?.ToUpper() ?? "EMAIL";

                await _unitOfWork.Notifications.Add(notification, cancellationToken);
                await _unitOfWork.Save(cancellationToken);

                var recipients = notification.To
                    .Select(o => o?.ToString())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Cast<string>()
                    .ToList();
                var successfulSends = 0;
                var failedRecipients = new List<string>();

                if (mode == "SMS")
                {
                    _logger.LogInformation("Queuing SMS notification for {RecipientCount} recipients", recipients.Count);
                    foreach (var recipient in recipients)
                    {
                        try
                        {
                            var trackingId = $"{notification.Id}_{recipient}";
                            var smsMessage = new SmsMessage
                            {
                                Id = Guid.NewGuid(),
                                To = recipient,
                                From = clientApplication.SmsSenderNumber,
                                Body = $"{notification.Title}: {notification.Message}",
                                TrackingId = trackingId
                            };

                            // Enqueue instead of sending directly
                            await _smsQueueService.EnqueueSmsAsync(smsMessage);
                            successfulSends++;

                            // Create history with Queued status
                            var history = new Domain.Models.NotificationHistory
                            {
                                Id = Guid.NewGuid(),
                                NotificationId = notification.Id,
                                SentDate = DateTime.UtcNow,
                                Status = "Queued",
                                Recipient = recipient,
                                NotificationType = "SMS",
                                CreatedDate = DateTime.UtcNow
                            };
                            await _unitOfWork.NotificationHistories.Add(history, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error queuing SMS to {Recipient}", recipient);
                            failedRecipients.Add(recipient);
                        }
                    }
                }
                else // Default to EMAIL
                {
                    _logger.LogInformation("Queuing individual notification emails to {RecipientCount} recipients", recipients.Count);

                    foreach (var recipient in recipients)
                    {
                        try
                        {
                            var trackingId = $"{notification.Id}_{recipient}";
                            var bodyHtml = $"<h2 style='margin-top: 0; margin-bottom: 10px; color: #333;'>{clientApplication.Name}.</h2><p style='margin-top: 0; margin-bottom: 20px; font-size: 16px; color: #555;'><em>{notification.Message}</em></p>";
                            var attachments = new List<EmailAttachment>();

                            // Handle logo as inline attachment if it's base64
                            if (!string.IsNullOrEmpty(clientApplication.Logo) && clientApplication.Logo.Contains("base64,"))
                            {
                                try 
                                {
                                    var parts = clientApplication.Logo.Split(',');
                                    var base64Data = parts[1];
                                    var contentType = parts[0].Split(':')[1].Split(';')[0];
                                    var extension = contentType.Split('/')[1];
                                    
                                    attachments.Add(new EmailAttachment 
                                    {
                                        FileName = $"logo.{extension}",
                                        Content = Convert.FromBase64String(base64Data),
                                        ContentType = contentType,
                                        IsInline = true,
                                        ContentId = "client_logo"
                                    });
                                    bodyHtml += "<img src='cid:client_logo' alt='Client Logo' style='max-width: 100%; width: auto; height: auto; max-height: 120px; display: block; margin: 20px 0; border: 0;' />";
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, "Failed to embed logo as CID, falling back to original logo source");
                                    bodyHtml += $"<img src='{clientApplication.Logo}' alt='Client Logo' style='max-width: 100%; width: auto; height: auto; max-height: 120px; display: block; margin: 20px 0; border: 0;' />";
                                }
                            }
                            else if (!string.IsNullOrEmpty(clientApplication.Logo))
                            {
                                bodyHtml += $"<img src='{clientApplication.Logo}' alt='Client Logo' style='max-width: 100%; width: auto; height: auto; max-height: 120px; display: block; margin: 20px 0; border: 0;' />";
                            }

                            // Create individual email message for each recipient
                            var individualEmailMessage = new EnhancedEmailMessage
                            {
                                To = new List<string> { recipient },
                                From = clientApplication.SenderEmail,
                                Subject = notification.Title,
                                BodyHtml = bodyHtml,
                                Attachments = attachments.Any() ? attachments : null,
                                TrackingId = trackingId,
                                EnableTracking = true,
                                Metadata = new Dictionary<string, object>
                                {
                                    { "SenderEmail", clientApplication.SenderEmail },
                                    { "AppPassword", clientApplication.AppPassword },
                                    { "ClientApplicationId", clientApplication.Id.ToString() },
                                    { "RecipientEmail", recipient }
                                }
                            };

                            // Queue the email instead of sending it directly
                            await _emailQueueService.QueueEmailAsync(individualEmailMessage);
                            successfulSends++;

                            // Create individual notification history for each recipient with Queued status
                            var notificationHistory = new Domain.Models.NotificationHistory
                            {
                                Id = Guid.NewGuid(),
                                NotificationId = notification.Id,
                                SentDate = DateTime.UtcNow,
                                Status = "Queued",
                                Recipient = recipient,
                                NotificationType = "EMAIL",
                                CreatedDate = DateTime.UtcNow,
                                IsDeleted = false
                            };

                            await _unitOfWork.NotificationHistories.Add(notificationHistory, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            failedRecipients.Add(recipient);
                            _logger.LogError(ex, "Error queuing notification email to {Recipient}", recipient);
                        }
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