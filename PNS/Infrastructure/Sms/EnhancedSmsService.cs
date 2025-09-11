// File Path: Infrastructure/Sms/EnhancedSmsService.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Sms
{
    public class EnhancedSmsService : ISmsService
    {
        private readonly IEnumerable<ISmsProvider> _smsProviders;
        private readonly ILogger<EnhancedSmsService> _logger;

        public EnhancedSmsService(IEnumerable<ISmsProvider> smsProviders, ILogger<EnhancedSmsService> logger)
        {
            _smsProviders = smsProviders.OrderByDescending(p => p.Priority);
            _logger = logger;
        }

        public async Task<bool> SendSmsAsync(SmsMessage smsMessage)
        {
            foreach (var provider in _smsProviders)
            {
                try
                {
                    _logger.LogInformation("Attempting to send SMS via {Provider} to {To}", provider.Name, smsMessage.To);
                    var result = await provider.SendSmsAsync(smsMessage);

                    if (result.IsSuccess)
                    {
                        _logger.LogInformation("SMS sent successfully via {Provider}, MessageId: {MessageId}", provider.Name, result.MessageId);

                        // Update message status
                        smsMessage.Status = "Sent";
                        smsMessage.SentAt = result.SentAt;
                        smsMessage.TrackingId = result.MessageId;

                        return true;
                    }
                    else
                    {
                        _logger.LogWarning("SMS failed via {Provider}: {Error}", provider.Name, result.ErrorMessage);
                        smsMessage.ErrorMessage = result.ErrorMessage;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while sending SMS via {Provider}", provider.Name);
                    smsMessage.ErrorMessage = ex.Message;
                }
            }

            _logger.LogError("All SMS providers failed to send the message to {To}", smsMessage.To);
            smsMessage.Status = "Failed";
            smsMessage.RetryCount++;

            return false;
        }
    }
}