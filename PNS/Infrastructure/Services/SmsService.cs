// File Path: Infrastructure/Services/SmsService.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SmsService : ISmsService
    {
        private readonly ILogger<SmsService> _logger;

        public SmsService(ILogger<SmsService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendSmsAsync(SmsMessage smsMessage)
        {
            try
            {
                _logger.LogInformation($"Sending SMS to {smsMessage.To}: {smsMessage.Body}");

                // Simulate SMS sending
                await Task.Delay(100);

                // Update message status
                smsMessage.Status = "Sent";
                smsMessage.SentAt = DateTime.UtcNow;

                _logger.LogInformation($"SMS sent successfully to {smsMessage.To}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send SMS to {smsMessage.To}");

                // Update message status
                smsMessage.Status = "Failed";
                smsMessage.ErrorMessage = ex.Message;
                smsMessage.RetryCount++;

                return false;
            }
        }
    }
}