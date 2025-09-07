// File Path: Infrastructure/Email/Providers/SmtpEmailProvider.cs
using Application.Models.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Email.Providers
{
    public class SmtpEmailProvider : IEmailProvider
    {
        private readonly SmtpProviderSettings _settings;

        public SmtpEmailProvider(IOptions<SmtpProviderSettings> settings)
        {
            _settings = settings.Value;
        }

        public string Name => "SMTP";
        public int Priority => 1;
        public bool IsConfigured => !string.IsNullOrEmpty(_settings.SmtpServer) && 
                                   !string.IsNullOrEmpty(_settings.Username) && 
                                   !string.IsNullOrEmpty(_settings.Password);

        public async Task<EmailSendResult> SendEmailAsync(EnhancedEmailMessage emailMessage)
        {
            try
            {
                using var smtpClient = new SmtpClient(_settings.SmtpServer)
                {
                    Port = _settings.Port,
                    Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                    EnableSsl = _settings.EnableSsl,
                    Timeout = _settings.Timeout
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailMessage.From),
                    Subject = emailMessage.Subject,
                    Body = emailMessage.BodyHtml,
                    IsBodyHtml = true
                };

                // Add recipients
                foreach (var recipient in emailMessage.To)
                {
                    mailMessage.To.Add(recipient);
                }

                // Add CC recipients
                if (emailMessage.Cc != null)
                {
                    foreach (var cc in emailMessage.Cc)
                    {
                        mailMessage.CC.Add(cc);
                    }
                }

                // Add BCC recipients
                if (emailMessage.Bcc != null)
                {
                    foreach (var bcc in emailMessage.Bcc)
                    {
                        mailMessage.Bcc.Add(bcc);
                    }
                }

                // Add attachments
                if (emailMessage.Attachments != null)
                {
                    foreach (var attachment in emailMessage.Attachments)
                    {
                        var stream = new MemoryStream(attachment.Content);
                        var mailAttachment = new Attachment(stream, attachment.FileName, attachment.ContentType);
                        if (attachment.IsInline && !string.IsNullOrEmpty(attachment.ContentId))
                        {
                            mailAttachment.ContentId = attachment.ContentId;
                        }
                        mailMessage.Attachments.Add(mailAttachment);
                    }
                }

                // Add custom headers
                if (emailMessage.Headers != null)
                {
                    foreach (var header in emailMessage.Headers)
                    {
                        mailMessage.Headers.Add(header.Key, header.Value);
                    }
                }

                await smtpClient.SendMailAsync(mailMessage);

                return EmailSendResult.Success(Guid.NewGuid().ToString(), Name);
            }
            catch (Exception ex)
            {
                return EmailSendResult.Failure(ex.Message, Name);
            }
        }

        public async Task<EmailSendResult> SendBulkEmailAsync(BulkEmailMessage bulkEmailMessage)
        {
            var tasks = new List<Task<EmailSendResult>>();
            var batches = bulkEmailMessage.Recipients
                .Select((recipient, index) => new { recipient, index })
                .GroupBy(x => x.index / bulkEmailMessage.BatchSize)
                .Select(g => g.Select(x => x.recipient).ToList());

            foreach (var batch in batches)
            {
                var emailMessage = new EnhancedEmailMessage
                {
                    From = bulkEmailMessage.From,
                    Subject = bulkEmailMessage.Subject,
                    BodyHtml = bulkEmailMessage.BodyHtml,
                    To = batch
                };

                tasks.Add(SendEmailAsync(emailMessage));
                
                if (bulkEmailMessage.BatchDelay > TimeSpan.Zero)
                {
                    await Task.Delay(bulkEmailMessage.BatchDelay);
                }
            }

            var results = await Task.WhenAll(tasks);
            var successCount = results.Count(r => r.IsSuccess);
            var totalCount = results.Length;

            if (successCount == totalCount)
            {
                return EmailSendResult.Success($"Bulk-{Guid.NewGuid()}", Name);
            }
            else
            {
                var failedCount = totalCount - successCount;
                return EmailSendResult.Failure($"Bulk send partially failed: {failedCount}/{totalCount} failed", Name);
            }
        }
    }

    public class SmtpProviderSettings
    {
        public required string SmtpServer { get; set; }
        public required int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool EnableSsl { get; set; } = true;
        public int Timeout { get; set; } = 30000;
    }
}