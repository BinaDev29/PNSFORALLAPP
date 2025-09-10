// File Path: Application/Services/EmailTemplateService.cs
using Application.Contracts.IRepository;
using AppEmailTemplate = Application.Models.Email.EmailTemplate;
using DomainEmailTemplate = Domain.Models.EmailTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IEmailTemplateService
    {
        Task<string> ProcessTemplateAsync(AppEmailTemplate template, Dictionary<string, object> data);
        Task<AppEmailTemplate?> GetTemplateAsync(string templateName);
        Task<string> ProcessNotificationTemplateAsync(Guid notificationId, Dictionary<string, object>? additionalData = null);
    }

    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly INotificationRepository _notificationRepository;

        public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository, INotificationRepository notificationRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<string> ProcessTemplateAsync(AppEmailTemplate template, Dictionary<string, object> data)
        {
            var processedHtml = template.HtmlBody;

            var regex = new Regex(@"\{\{(\w+)\}\}", RegexOptions.IgnoreCase);
            processedHtml = regex.Replace(processedHtml, match =>
            {
                var variableName = match.Groups[1].Value;
                return data.TryGetValue(variableName, out var value) ? value?.ToString() ?? "" : match.Value;
            });

            return processedHtml;
        }

        public async Task<AppEmailTemplate?> GetTemplateAsync(string templateName)
        {
            // Fix: Change 'Find' to a valid repository method. Assuming 'GetWhere' is the correct method.
            // Also, your repository should accept a CancellationToken.
            var templates = await _emailTemplateRepository.GetWhere(t => t.Name == templateName, CancellationToken.None);
            var template = templates.FirstOrDefault();

            if (template == null) return null;

            return new AppEmailTemplate
            {
                Id = template.Id,
                Name = template.Name,
                Subject = template.Subject,
                HtmlBody = template.BodyHtml,
                RequiredVariables = ExtractVariables(template.BodyHtml)
            };
        }

        public async Task<string> ProcessNotificationTemplateAsync(Guid notificationId, Dictionary<string, object>? additionalData = null)
        {
            var notification = await _notificationRepository.Get(notificationId, CancellationToken.None);
            if (notification == null) throw new ArgumentException("Notification not found");

            var data = new Dictionary<string, object>
            {
                ["Title"] = notification.Title,
                ["Message"] = notification.Message,
                ["CreatedDate"] = notification.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                ["NotificationId"] = notification.Id,
                ["TrackingUrl"] = $"https://localhost:7198/api/Notification/{notification.Id}/track"
            };

            if (additionalData != null)
            {
                foreach (var item in additionalData)
                {
                    data[item.Key] = item.Value;
                }
            }

            var template = await GetTemplateAsync("DefaultNotification");
            if (template is null)
            {
                return $@"
                    <html>
                    <body>
                        <h2>{notification.Title}</h2>
                        <p>{notification.Message}</p>
                        <p><small>Sent on {notification.CreatedDate:yyyy-MM-dd HH:mm:ss}</small></p>
                        <img src='{data["TrackingUrl"]}' style='display:none;' />
                    </body>
                    </html>";
            }

            return await ProcessTemplateAsync(template, data);
        }

        private List<string> ExtractVariables(string template)
        {
            var variables = new List<string>();
            var regex = new Regex(@"\{\{(\w+)\}\}", RegexOptions.IgnoreCase);
            var matches = regex.Matches(template);

            foreach (Match match in matches)
            {
                var variableName = match.Groups[1].Value;
                if (!variables.Contains(variableName))
                {
                    variables.Add(variableName);
                }
            }
            return variables;
        }
    }
}