// File Path: Infrastructure/Email/Providers/SendGridEmailProvider.cs
using Application.Models.Email;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Email.Providers
{
    public class SendGridEmailProvider : IEmailProvider
    {
        private readonly SendGridSettings _settings;
        private readonly ISendGridClient _client;

        public SendGridEmailProvider(IOptions<SendGridSettings> settings)
        {
            _settings = settings.Value;
            _client = new SendGridClient(_settings.ApiKey);
        }

        public string Name => "SendGrid";
        public int Priority => 2;
        public bool IsConfigured => !string.IsNullOrEmpty(_settings.ApiKey);

        public async Task<EmailSendResult> SendEmailAsync(EnhancedEmailMessage emailMessage)
        {
            try
            {
                var from = new EmailAddress(emailMessage.From);
                var to = emailMessage.To.Select(email => new EmailAddress(email)).ToList();

                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(
                    from, to, emailMessage.Subject, null, emailMessage.BodyHtml);

                // Add CC recipients
                if (emailMessage.Cc != null && emailMessage.Cc.Any())
                {
                    msg.AddCcs(emailMessage.Cc.Select(email => new EmailAddress(email)).ToList());
                }

                // Add BCC recipients
                if (emailMessage.Bcc != null && emailMessage.Bcc.Any())
                {
                    msg.AddBccs(emailMessage.Bcc.Select(email => new EmailAddress(email)).ToList());
                }

                // Add attachments
                if (emailMessage.Attachments != null)
                {
                    foreach (var attachment in emailMessage.Attachments)
                    {
                        var base64Content = Convert.ToBase64String(attachment.Content);
                        msg.AddAttachment(attachment.FileName, base64Content, attachment.ContentType);
                    }
                }

                // Add custom headers
                if (emailMessage.Headers != null)
                {
                    foreach (var header in emailMessage.Headers)
                    {
                        msg.AddHeader(header.Key, header.Value);
                    }
                }

                var response = await _client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    var messageId = response.Headers.GetValues("X-Message-Id").FirstOrDefault() ?? Guid.NewGuid().ToString();
                    return EmailSendResult.Success(messageId, Name);
                }
                else
                {
                    var errorBody = await response.Body.ReadAsStringAsync();
                    return EmailSendResult.Failure($"SendGrid error: {response.StatusCode} - {errorBody}", Name);
                }
            }
            catch (Exception ex)
            {
                return EmailSendResult.Failure(ex.Message, Name);
            }
        }

        public async Task<EmailSendResult> SendBulkEmailAsync(BulkEmailMessage bulkEmailMessage)
        {
            try
            {
                var from = new EmailAddress(bulkEmailMessage.From);
                var recipients = bulkEmailMessage.Recipients.Select(email => new EmailAddress(email)).ToList();

                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(
                    from, recipients, bulkEmailMessage.Subject, bulkEmailMessage.BodyText, bulkEmailMessage.BodyHtml);

                var response = await _client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    var messageId = response.Headers.GetValues("X-Message-Id").FirstOrDefault() ?? $"Bulk-{Guid.NewGuid()}";
                    return EmailSendResult.Success(messageId, Name);
                }
                else
                {
                    var errorBody = await response.Body.ReadAsStringAsync();
                    return EmailSendResult.Failure($"SendGrid bulk error: {response.StatusCode} - {errorBody}", Name);
                }
            }
            catch (Exception ex)
            {
                return EmailSendResult.Failure(ex.Message, Name);
            }
        }
    }

    public class SendGridSettings
    {
        public required string ApiKey { get; set; }
    }
}