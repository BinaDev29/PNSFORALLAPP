// File Path: Infrastructure/Email/EmailService.cs
using Application.Contracts;
using Application.Models.Email;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Infrastructure.Email
{
    public class SmtpSettings
    {
        public required string SmtpServer { get; set; }
        public required int Port { get; set; }
        public required string AppEmail { get; set; }
        public required string AppPassword { get; set; }
    }

    public class EmailService(IOptions<SmtpSettings> smtpSettings) : IEmailService
    {
        private readonly SmtpSettings _smtpSettings = smtpSettings.Value;

        // FIX: The method signature now matches the IEmailService interface.
        public async Task<bool> SendEmail(EnhancedEmailMessage emailMessage)
        {
            try
            {
                using var smtpClient = new SmtpClient(_smtpSettings.SmtpServer)
                {
                    Port = _smtpSettings.Port,
                    // FIX: Credentials are now loaded from the SmtpSettings configuration.
                    Credentials = new NetworkCredential(_smtpSettings.AppEmail, _smtpSettings.AppPassword),
                    EnableSsl = true,
                };

                // FIX: Use the new TrackingUrl from the EnhancedEmailMessage model.
                var htmlBodyWithTrackingPixel = emailMessage.BodyHtml;
                if (!string.IsNullOrEmpty(emailMessage.TrackingId) && emailMessage.EnableTracking)
                {
                    var trackingUrl = $"https://localhost:7198/api/Notification/{emailMessage.TrackingId}/track";
                    htmlBodyWithTrackingPixel = $"{emailMessage.BodyHtml}<img src='{trackingUrl}' style='display:none;' />";
                }

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailMessage.From),
                    Subject = emailMessage.Subject,
                    Body = htmlBodyWithTrackingPixel,
                    IsBodyHtml = true
                };

                // FIX: Iterate through the recipient list from the EnhancedEmailMessage object.
                foreach (var recipient in emailMessage.To)
                {
                    mailMessage.To.Add(recipient);
                }

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (SmtpException ex)
            {
                // In a production app, use a logger instead of Console.WriteLine
                Console.WriteLine($"SMTP Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return false;
            }
        }

        // FIX: Implement the SendBulkEmailAsync method from the IEmailService interface.
        public Task<bool> SendBulkEmailAsync(BulkEmailMessage bulkEmailMessage, string senderEmail, string appPassword)
        {
            // TODO: Add implementation for sending bulk emails
            throw new NotImplementedException();
        }

        // FIX: Implement the SendTemplatedEmailAsync method from the IEmailService interface.
        public Task<bool> SendTemplatedEmailAsync(
            string templateName,
            Dictionary<string, object> templateData,
            List<string> recipients,
            string senderEmail,
            string subject,
            string from,
            string appPassword)
        {
            // TODO: Add implementation for sending templated emails
            throw new NotImplementedException();
        }
    }
}