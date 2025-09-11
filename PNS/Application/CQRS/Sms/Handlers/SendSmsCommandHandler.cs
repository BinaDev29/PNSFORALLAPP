// File Path: Application/CQRS/Sms/Handlers/SendSmsCommandHandler.cs
using Application.CQRS.Sms.Commands;
using Application.Contracts;
using Application.Contracts.IRepository;
using Application.Responses;
using Domain.Events;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Sms.Handlers
{
    public class SendSmsCommandHandler : IRequestHandler<SendSmsCommand, BaseCommandResponse>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ISmsService _smsService;
        private readonly ISmsTemplateRepository _smsTemplateRepository;
        private readonly ILogger<SendSmsCommandHandler> _logger;

        public SendSmsCommandHandler(
            INotificationRepository notificationRepository,
            ISmsService smsService,
            ISmsTemplateRepository smsTemplateRepository,
            ILogger<SendSmsCommandHandler> logger)
        {
            _notificationRepository = notificationRepository;
            _smsService = smsService;
            _smsTemplateRepository = smsTemplateRepository;
            _logger = logger;
        }

        public async Task<BaseCommandResponse> Handle(SendSmsCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var dto = request.SendSmsDto;

                // Validate phone number - use the correct property name
                var phoneNumber = PhoneNumber.Create(dto.PhoneNumber);

                // Process SMS template if provided - use correct property names
                string messageBody = dto.Message;
                if (!string.IsNullOrEmpty(dto.TemplateName))
                {
                    var template = await _smsTemplateRepository.GetByNameAsync(dto.TemplateName);
                    if (template != null)
                    {
                        messageBody = ProcessTemplate(template.Body, dto.TemplateData);
                    }
                }

                // Create notification entity - use correct property names
                var recipients = new List<string> { phoneNumber.Value };
                var notification = Domain.Models.Notification.CreateNotification(
                    dto.ClientApplicationId,
                    recipients,
                    dto.Title,
                    messageBody,
                    dto.PriorityId,
                    dto.NotificationTypeId,
                    dto.ScheduledAt
                );

                // Save notification to database
                await _notificationRepository.Add(notification);

                // Send SMS if not scheduled for future
                if (notification.IsReadyToSend())
                {
                    // Create SmsMessage using Domain model
                    var smsMessage = new SmsMessage
                    {
                        To = phoneNumber.Value,
                        Body = messageBody,
                        TrackingId = notification.Id.ToString(),
                        MaxRetries = 3
                    };

                    var smsResult = await _smsService.SendSmsAsync(smsMessage);

                    if (smsResult)
                    {
                        notification.MarkAsSent();
                        notification.AddDomainEvent(new SmsNotificationSentEvent(
                            notification.Id, phoneNumber.Value, messageBody,
                            smsMessage.TrackingId, true));

                        _logger.LogInformation("SMS notification sent successfully to {PhoneNumber}", phoneNumber.Value);
                    }
                    else
                    {
                        notification.MarkAsFailed("Failed to send SMS");
                        notification.AddDomainEvent(new SmsNotificationSentEvent(
                            notification.Id, phoneNumber.Value, messageBody,
                            smsMessage.TrackingId, false, "Failed to send SMS"));

                        _logger.LogError("Failed to send SMS notification to {PhoneNumber}", phoneNumber.Value);
                    }

                    await _notificationRepository.Update(notification);
                }

                response.Success = true;
                response.Message = "SMS notification processed successfully";
                response.Id = notification.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error processing SMS notification: {ex.Message}";
                response.Errors = new List<string> { ex.Message };
                _logger.LogError(ex, "Error processing SMS notification");
            }

            return response;
        }

        private string ProcessTemplate(string template, Dictionary<string, string>? templateData)
        {
            if (templateData == null) return template;

            var processedTemplate = template;
            foreach (var kvp in templateData)
            {
                processedTemplate = processedTemplate.Replace($"{{{kvp.Key}}}", kvp.Value);
            }
            return processedTemplate;
        }
    }
}