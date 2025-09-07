// File Path: Infrastructure/Email/Providers/IEmailProvider.cs
using Application.Models.Email;
using System.Threading.Tasks;

namespace Infrastructure.Email.Providers
{
    public interface IEmailProvider
    {
        string Name { get; }
        Task<EmailSendResult> SendEmailAsync(EnhancedEmailMessage emailMessage);
        Task<EmailSendResult> SendBulkEmailAsync(BulkEmailMessage bulkEmailMessage);
        bool IsConfigured { get; }
        int Priority { get; }
    }

    public class EmailSendResult
    {
        public bool IsSuccess { get; set; }
        public string? MessageId { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Provider { get; set; }
        public DateTime SentAt { get; set; }

        public static EmailSendResult Success(string messageId, string provider)
        {
            return new EmailSendResult
            {
                IsSuccess = true,
                MessageId = messageId,
                Provider = provider,
                SentAt = DateTime.UtcNow
            };
        }

        public static EmailSendResult Failure(string errorMessage, string provider)
        {
            return new EmailSendResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                Provider = provider,
                SentAt = DateTime.UtcNow
            };
        }
    }
}