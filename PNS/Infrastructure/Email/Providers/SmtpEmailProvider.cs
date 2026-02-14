// File Path: Infrastructure/Email/Providers/SmtpEmailProvider.cs
using Application.Models.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Email.Providers
{
    public class SmtpEmailProvider : IEmailProvider
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<SmtpEmailProvider> _logger;

        public SmtpEmailProvider(IOptions<SmtpSettings> smtpSettings, ILogger<SmtpEmailProvider> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public string Name => "SMTP";
        public int Priority => 1;
        public bool IsConfigured => !string.IsNullOrEmpty(_smtpSettings.Host) && _smtpSettings.Port > 0;

        public async Task<EmailSendResult> SendEmailAsync(EnhancedEmailMessage emailMessage)
        {
            try
            {
                // Use client application credentials from metadata if available, otherwise fallback to SMTP settings
                var senderEmail = GetClientSenderEmail(emailMessage) ?? _smtpSettings.SenderEmail;
                var senderPassword = GetClientAppPassword(emailMessage) ?? _smtpSettings.SenderPassword;

                if (string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderPassword))
                {
                    var errorMsg = "SMTP configuration incomplete: SenderEmail and AppPassword must be provided either in ClientApplication or SMTP settings";
                    _logger.LogError(errorMsg);
                    return EmailSendResult.Failure(errorMsg, Name);
                }

                // Use SMTP settings for host, port, SSL but client credentials for authentication
                using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    EnableSsl = _smtpSettings.EnableSsl,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, _smtpSettings.SenderName ?? senderEmail),
                    Subject = emailMessage.Subject,
                    Body = emailMessage.BodyHtml,
                    IsBodyHtml = true
                };

                // Add recipients
                foreach (var recipient in emailMessage.To)
                {
                    mailMessage.To.Add(recipient);
                }

                // Add attachments and handle inline images
                if (emailMessage.Attachments != null && emailMessage.Attachments.Any())
                {
                    foreach (var attachment in emailMessage.Attachments)
                    {
                        var ms = new System.IO.MemoryStream(attachment.Content);
                        if (attachment.IsInline && !string.IsNullOrEmpty(attachment.ContentId))
                        {
                            var inlineAttachment = new Attachment(ms, attachment.FileName, attachment.ContentType);
                            inlineAttachment.ContentId = attachment.ContentId;
                            inlineAttachment.ContentDisposition.Inline = true;
                            inlineAttachment.ContentDisposition.DispositionType = "inline";
                            mailMessage.Attachments.Add(inlineAttachment);
                        }
                        else
                        {
                            var ordinaryAttachment = new Attachment(ms, attachment.FileName, attachment.ContentType);
                            mailMessage.Attachments.Add(ordinaryAttachment);
                        }
                    }
                }

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully via SMTP using client credentials {SenderEmail} to {Recipients}",
                    senderEmail, string.Join(", ", emailMessage.To));

                return EmailSendResult.Success(Guid.NewGuid().ToString(), Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email via SMTP: {Message}", ex.Message);
                return EmailSendResult.Failure(ex.Message, Name);
            }
        }

        public async Task<EmailSendResult> SendBulkEmailAsync(BulkEmailMessage bulkEmailMessage)
        {
            try
            {
                // For bulk email, use the From field or fallback to SMTP settings
                var senderEmail = bulkEmailMessage.From ?? _smtpSettings.SenderEmail;
                var senderPassword = _smtpSettings.SenderPassword; // For bulk, use SMTP settings password

                if (string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderPassword))
                {
                    var errorMsg = "SMTP configuration incomplete for bulk email: SenderEmail and SenderPassword required";
                    _logger.LogError(errorMsg);
                    return EmailSendResult.Failure(errorMsg, Name);
                }

                using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    EnableSsl = _smtpSettings.EnableSsl,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };

                foreach (var recipient in bulkEmailMessage.Recipients)
                {
                    using var mailMessage = new MailMessage
                    {
                        From = new MailAddress(senderEmail, _smtpSettings.SenderName ?? senderEmail),
                        Subject = bulkEmailMessage.Subject,
                        Body = bulkEmailMessage.BodyHtml,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(recipient);
                    await client.SendMailAsync(mailMessage);
                }

                _logger.LogInformation("Bulk email sent successfully via SMTP to {Count} recipients using {SenderEmail}",
                    bulkEmailMessage.Recipients.Count, senderEmail);
                return EmailSendResult.Success(Guid.NewGuid().ToString(), Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send bulk email via SMTP: {Message}", ex.Message);
                return EmailSendResult.Failure(ex.Message, Name);
            }
        }

        private string? GetClientSenderEmail(EnhancedEmailMessage emailMessage)
        {
            // First check if SenderEmail is provided in metadata (from ClientApplication)
            if (emailMessage.Metadata?.ContainsKey("SenderEmail") == true)
            {
                return emailMessage.Metadata["SenderEmail"]?.ToString();
            }

            // Then check the From field
            return emailMessage.From;
        }

        private string? GetClientAppPassword(EnhancedEmailMessage emailMessage)
        {
            // Check if AppPassword is provided in metadata (from ClientApplication)
            if (emailMessage.Metadata?.ContainsKey("AppPassword") == true)
            {
                return emailMessage.Metadata["AppPassword"]?.ToString();
            }

            return null;
        }
    }

    public class SmtpSettings
    {
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string? SenderEmail { get; set; }
        public string? SenderPassword { get; set; }
        public string? SenderName { get; set; }
    }
}