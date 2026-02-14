// File Path: Infrastructure/Sms/TwilioSmsProvider.cs
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

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
                var globalFromNumber = _configuration["Twilio:PhoneNumber"];
                
                // Use message-specific 'From' if available, otherwise global
                var fromNumber = !string.IsNullOrEmpty(smsMessage.From) ? smsMessage.From : globalFromNumber;

                if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(fromNumber))
                {
                    _logger.LogError("Twilio configuration is missing. AccountSid: {Sid}, From: {From}", 
                        string.IsNullOrEmpty(accountSid) ? "Missing" : "Present",
                        string.IsNullOrEmpty(fromNumber) ? "Missing" : fromNumber);
                    return SmsSendResult.Failure("Twilio configuration is missing");
                }

                _logger.LogInformation("Sending SMS via Twilio to {To}", smsMessage.To);

                // Initialize Twilio Client
                TwilioClient.Init(accountSid, authToken);

                var message = await MessageResource.CreateAsync(
                    body: smsMessage.Body,
                    from: new PhoneNumber(fromNumber),
                    to: new PhoneNumber(smsMessage.To)
                );

                if (message.ErrorCode != null)
                {
                    _logger.LogError("Twilio Error: {Code} - {Message}", message.ErrorCode, message.ErrorMessage);
                    return SmsSendResult.Failure(message.ErrorMessage);
                }

                _logger.LogInformation("SMS sent successfully via Twilio, SID: {Sid}", message.Sid);
                return SmsSendResult.Success(message.Sid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS via Twilio to {To}", smsMessage.To);
                return SmsSendResult.Failure(ex.Message);
            }
        }
    }
}