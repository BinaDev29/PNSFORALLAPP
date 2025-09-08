using Application.Models.Email;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Email.Providers
{
    public class SmtpEmailProvider : IEmailProvider
    {
        public string Name => "SMTP";
        public int Priority => 1;
        public bool IsConfigured => true;

        public async Task<EmailSendResult> SendEmailAsync(EnhancedEmailMessage emailMessage)
        {
            // Extract credentials from Metadata
            if (emailMessage.Metadata == null ||
                !emailMessage.Metadata.TryGetValue("SenderEmail", out var senderEmailObj) ||
                !emailMessage.Metadata.TryGetValue("AppPassword", out var appPasswordObj))
            {
                return EmailSendResult.Failure("SenderEmail and AppPassword must be provided in Metadata.", Name);
            }

            var senderEmail = senderEmailObj?.ToString();
            var appPassword = appPasswordObj?.ToString();

            if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(appPassword))
            {
                return EmailSendResult.Failure("SenderEmail or AppPassword is empty.", Name);
            }

            try
            {
                using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(senderEmail, appPassword),
                    EnableSsl = true
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = emailMessage.Subject,
                    Body = emailMessage.BodyHtml,
                    IsBodyHtml = true
                };

                foreach (var to in emailMessage.To)
                    mailMessage.To.Add(to);

                if (emailMessage.Cc != null)
                    foreach (var cc in emailMessage.Cc)
                        mailMessage.CC.Add(cc);

                if (emailMessage.Bcc != null)
                    foreach (var bcc in emailMessage.Bcc)
                        mailMessage.Bcc.Add(bcc);

                if (emailMessage.Attachments != null)
                {
                    foreach (var attachment in emailMessage.Attachments)
                    {
                        var stream = new System.IO.MemoryStream(attachment.Content);
                        mailMessage.Attachments.Add(new Attachment(stream, attachment.FileName, attachment.ContentType));
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
            return await Task.FromResult(
                EmailSendResult.Failure("Bulk email via SMTP requires sender credentials in Metadata.", Name)
            );
        }
    }
}