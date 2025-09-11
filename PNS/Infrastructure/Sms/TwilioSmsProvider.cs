// File Path: Infrastructure/Sms/TwilioSmsProvider.cs
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Sms
{
    public class TwilioSmsProvider : ISmsProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TwilioSmsProvider> _logger;

        public string Name => "Twilio";
        public int Priority => 1;

        public TwilioSmsProvider(IConfiguration configuration, ILogger<TwilioSmsProvider> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<SmsSendResult> SendSmsAsync(SmsMessage smsMessage)
        {
            try
            {
                // Get Twilio configuration
                var accountSid = _configuration["Twilio:AccountSid"];
                var authToken = _configuration["Twilio:AuthToken"];
                var fromNumber = _configuration["Twilio:FromNumber"];

                if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(fromNumber))
                {
                    _logger.LogError("Twilio configuration is missing");
                    return SmsSendResult.Failure("Twilio configuration is missing");
                }

                _logger.LogInformation("Sending SMS via Twilio to {To}", smsMessage.To);

                // TODO: Implement actual Twilio SMS sending
                // For now, simulate the sending
                await Task.Delay(500);

                // Simulate success/failure based on phone number format
                if (smsMessage.To.StartsWith("+1") || smsMessage.To.StartsWith("1"))
                {
                    var messageId = $"twilio_{Guid.NewGuid():N}";
                    _logger.LogInformation("SMS sent successfully via Twilio, MessageId: {MessageId}", messageId);
                    return SmsSendResult.Success(messageId);
                }
                else
                {
                    _logger.LogWarning("Invalid phone number format for Twilio: {To}", smsMessage.To);
                    return SmsSendResult.Failure("Invalid phone number format");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS via Twilio to {To}", smsMessage.To);
                return SmsSendResult.Failure(ex.Message);
            }
        }
    }
}