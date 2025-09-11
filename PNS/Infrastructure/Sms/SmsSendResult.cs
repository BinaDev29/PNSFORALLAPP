// File Path: Infrastructure/Sms/SmsSendResult.cs
using System;

namespace Infrastructure.Sms
{
    public class SmsSendResult
    {
        public bool IsSuccess { get; set; }
        public string? MessageId { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public static SmsSendResult Success(string messageId)
        {
            return new SmsSendResult
            {
                IsSuccess = true,
                MessageId = messageId,
                SentAt = DateTime.UtcNow
            };
        }

        public static SmsSendResult Failure(string errorMessage)
        {
            return new SmsSendResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                SentAt = DateTime.UtcNow
            };
        }
    }
}