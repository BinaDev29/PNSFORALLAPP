// File Path: Infrastructure/Sms/ISmsProvider.cs
using Application.Models.Sms;
using System.Threading.Tasks;

namespace Infrastructure.Sms
{
    public interface ISmsProvider
    {
        string Name { get; }
        int Priority { get; }
        Task<SmsSendResult> SendSmsAsync(SmsMessage smsMessage);
    }

    public class SmsSendResult
    {
        public bool IsSuccess { get; set; }
        public string? MessageId { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Provider { get; set; }
        public DateTime SentAt { get; set; }
        public static SmsSendResult Success(string messageId, string provider)
        {
            return new SmsSendResult { IsSuccess = true, MessageId = messageId, Provider = provider, SentAt = DateTime.UtcNow };
        }
        public static SmsSendResult Failure(string errorMessage, string provider)
        {
            return new SmsSendResult { IsSuccess = false, ErrorMessage = errorMessage, Provider = provider, SentAt = DateTime.UtcNow };
        }
    }
}