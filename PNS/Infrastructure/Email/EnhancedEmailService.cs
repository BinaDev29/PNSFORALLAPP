using Application.Contracts;
using Application.Models.Email;
using Application.Services;
using Infrastructure.Email.Providers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class EnhancedEmailService : IEmailService
    {
        private readonly IEnumerable<IEmailProvider> _emailProviders;
        private readonly IEmailTemplateService _templateService;
        private readonly ILogger<EnhancedEmailService> _logger;

        public EnhancedEmailService(
            IEnumerable<IEmailProvider> emailProviders,
            IEmailTemplateService templateService,
            ILogger<EnhancedEmailService> logger)
        {
            _emailProviders = emailProviders.OrderByDescending(p => p.Priority);
            _templateService = templateService;
            _logger = logger;
        }

        public async Task<bool> SendEmail(EmailMessage emailMessage, Guid notificationId, string appemail, string apppassword)
        {
            try
            {
                var enhancedEmail = new EnhancedEmailMessage
                {
                    From = emailMessage.From,
                    To = emailMessage.To,
                    Subject = emailMessage.Subject,
                    BodyHtml = emailMessage.BodyHtml,
                    TrackingId = notificationId.ToString(),
                    EnableTracking = true
                };

                if (enhancedEmail.EnableTracking)
                {
                    var trackingUrl = $"https://localhost:7198/api/Notification/{notificationId}/track";
                    enhancedEmail.BodyHtml += $"<img src='{trackingUrl}' style='display:none;' />";
                }

                return await SendEnhancedEmailAsync(enhancedEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email for notification {NotificationId}", notificationId);
                return false;
            }
        }

        public async Task<bool> SendEnhancedEmailAsync(EnhancedEmailMessage emailMessage)
        {
            var availableProviders = _emailProviders.Where(p => p.IsConfigured).ToList();

            if (!availableProviders.Any())
            {
                _logger.LogError("No email providers are configured");
                return false;
            }

            foreach (var provider in availableProviders)
            {
                try
                {
                    _logger.LogInformation("Attempting to send email via {Provider}", provider.Name);
                    var result = await provider.SendEmailAsync(emailMessage);
                    if (result.IsSuccess)
                    {
                        _logger.LogInformation("Email sent successfully via {Provider}, MessageId: {MessageId}", provider.Name, result.MessageId);
                        return true;
                    }
                    else
                    {
                        _logger.LogWarning("Email failed via {Provider}: {Error}", provider.Name, result.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while sending email via {Provider}", provider.Name);
                }
            }

            _logger.LogError("All email providers failed to send email");
            return false;
        }

        public async Task<bool> SendBulkEmailAsync(BulkEmailMessage bulkEmailMessage)
        {
            var availableProviders = _emailProviders.Where(p => p.IsConfigured).ToList();

            if (!availableProviders.Any())
            {
                _logger.LogError("No email providers are configured for bulk email");
                return false;
            }

            foreach (var provider in availableProviders)
            {
                try
                {
                    _logger.LogInformation("Attempting to send bulk email via {Provider} to {RecipientCount} recipients", provider.Name, bulkEmailMessage.Recipients.Count);
                    var result = await provider.SendBulkEmailAsync(bulkEmailMessage);
                    if (result.IsSuccess)
                    {
                        _logger.LogInformation("Bulk email sent successfully via {Provider}, MessageId: {MessageId}", provider.Name, result.MessageId);
                        return true;
                    }
                    else
                    {
                        _logger.LogWarning("Bulk email failed via {Provider}: {Error}", provider.Name, result.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while sending bulk email via {Provider}", provider.Name);
                }
            }

            return false;
        }

        public async Task<bool> SendTemplatedEmailAsync(string templateName, Dictionary<string, object> templateData, List<string> recipients, string from, string subject)
        {
            try
            {
                var template = await _templateService.GetTemplateAsync(templateName);
                if (template == null)
                {
                    _logger.LogError("Email template '{TemplateName}' not found", templateName);
                    return false;
                }

                var processedHtml = await _templateService.ProcessTemplateAsync(template, templateData);

                var emailMessage = new EnhancedEmailMessage
                {
                    From = from,
                    To = recipients,
                    Subject = subject,
                    BodyHtml = processedHtml
                };

                return await SendEnhancedEmailAsync(emailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send templated email using template {TemplateName}", templateName);
                return false;
            }
        }
    }
}