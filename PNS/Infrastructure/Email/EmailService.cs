// File Path: Infrastructure/Email/EmailService.cs
using Application.Contracts;
using Application.Models.Email;
using Microsoft.Extensions.Options;
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

        public async Task<bool> SendEmail(EmailMessage email)
        {
            try
            {
                using var smtpClient = new SmtpClient(_smtpSettings.SmtpServer)
                {
                    Port = _smtpSettings.Port,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true,
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.Username),
                    Subject = email.Subject,
                    Body = email.Body,
                    IsBodyHtml = true // ይህ መስመር መልእክቱ እንደ HTML እንዲነበብ ያደርጋል
                };

                // ለእያንዳንዱ ተቀባይ ኢሜይሉን ወደ 'To' field እንጨምራለን
                foreach (var recipient in email.To)
                {
                    mailMessage.To.Add(recipient);
                }

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (SmtpException ex)
            {
                // ለምርመራ እንዲረዳህ ስህተቱን ወደ ኮንሶል ማሳየት ትችላለህ
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