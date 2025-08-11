// File Path: Infrastructure/Email/EmailService.cs
using Application.Contracts;
using Application.Models.Email;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class SmtpSettings
    {
        public required string SmtpServer { get; set; }
        public required int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class EmailService(IOptions<SmtpSettings> smtpSettings) : IEmailService
    {
        private readonly SmtpSettings _smtpSettings = smtpSettings.Value;

        public async Task<bool> SendEmail(EmailMessage email, Guid notificationId, string appemail, string apppassword)
        {
            try
            {
                using var smtpClient = new SmtpClient(_smtpSettings.SmtpServer)
                {
                    Port = _smtpSettings.Port,
                    Credentials = new NetworkCredential(appemail, apppassword), // የPassed email እና passwordን ይጠቀማል
                    EnableSsl = true,
                };

                var trackingUrl = $"https://localhost:7198/api/Notification/{notificationId}/track";
                var htmlBodyWithTrackingPixel = $"{email.BodyHtml}<img src='{trackingUrl}' style='display:none;' />";

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(email.From),
                    Subject = email.Subject,
                    Body = htmlBodyWithTrackingPixel,
                    IsBodyHtml = true
                };

                foreach (var recipient in email.To)
                {
                    mailMessage.To.Add(recipient);
                }

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return false;
            }
        }
    }
}