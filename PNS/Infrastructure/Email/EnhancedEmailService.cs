// File Path: Infrastructure/Email/EnhancedEmailService.cs
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

        // FIX: The method signature now correctly implements IEmailService.
        // It accepts a single EnhancedEmailMessage object, which should contain all necessary data.
        public async Task<bool> SendEmail(EnhancedEmailMessage emailMessage)
        {
            try
            {
                // Note: The logic to add the tracking pixel should be in a single place, 
                // ideally within the EmailTemplateService or a dedicated decorator.
                // For now, it's kept here for demonstration.
                if (emailMessage.EnableTracking && !string.IsNullOrEmpty(emailMessage.TrackingId))
                {
                    var trackingUrl = $"https://localhost:7198/api/Notification/{emailMessage.TrackingId}/track";
                    emailMessage.BodyHtml += $"<img src='{trackingUrl}' style='display:none;' />";
                }

                // Call the internal method to handle the provider failover logic.
                return await SendEmailWithProviderFailoverAsync(emailMessage);
            }
            catch (Exception ex)
            {
                // This is a more appropriate place for a general catch-all log.
                _logger.LogError(ex, "Failed to send email: {Subject}", emailMessage.Subject);
                return false;
            }
        }

        // This is a new, private method to handle the provider failover logic.
        private async Task<bool> SendEmailWithProviderFailoverAsync(EnhancedEmailMessage emailMessage)
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

        // This method can be part of a separate IBulkEmailService interface if needed.
        public async Task<bool> SendBulkEmailAsync(BulkEmailMessage bulkEmailMessage, string clientAppSenderEmail, string clientAppPassword)
        {
            var availableProviders = _emailProviders.Where(p => p.IsConfigured).ToList();

            if (!availableProviders.Any())
            {
                _logger.LogError("No email providers are configured for bulk email");
                return false;
            }

            // Set sender email and credentials in each bulk message if needed
            bulkEmailMessage.From = clientAppSenderEmail;
            // If your provider supports credentials via Metadata, you can add:
            // bulkEmailMessage.Metadata = new Dictionary<string, object>
            // {
            //      { "SenderEmail", clientAppSenderEmail },
            //      { "AppPassword", clientAppPassword }
            // };

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

        // This method can also be part of a separate ITemplatedEmailService interface.
        public async Task<bool> SendTemplatedEmailAsync(string templateName, Dictionary<string, object> templateData, List<string> recipients, string from, string subject, string clientAppSenderEmail = null, string clientAppPassword = null)
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
                    From = clientAppSenderEmail ?? from,
                    To = recipients,
                    Subject = subject,
                    BodyHtml = processedHtml,
                    Metadata = (clientAppSenderEmail != null && clientAppPassword != null)
                        ? new Dictionary<string, object>
                        {
                            { "SenderEmail", clientAppSenderEmail },
                            { "AppPassword", clientAppPassword }
                        }
                        : null
                };

                return await SendEmailWithProviderFailoverAsync(emailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send templated email using template {TemplateName}", templateName);
                return false;
            }
        }
    }
}