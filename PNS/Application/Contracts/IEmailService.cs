// File Path: Application/Contracts/IEmailService.cs

using Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IEmailService
    {
        // Method to send a single, enhanced email message.
        Task<bool> SendEmail(EnhancedEmailMessage emailMessage);

        // Method to send emails to multiple recipients in bulk.
        // It requires the bulk email message, sender's email, and app password.
        Task<bool> SendBulkEmailAsync(BulkEmailMessage bulkEmailMessage, string senderEmail, string appPassword);

        // Method to send emails using a pre-defined template.
        // It requires the template name, template data, recipient list, subject, sender's email, and app password.
        Task<bool> SendTemplatedEmailAsync(
            string templateName,
            Dictionary<string, object> templateData,
            List<string> recipients,
            string senderEmail,
            string subject,
            string from,
            string appPassword);
    }
}