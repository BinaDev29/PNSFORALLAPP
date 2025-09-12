// File Path: API/Controllers/EnhancedNotificationController.cs
using Application.Common.Interfaces;
using Application.Contracts;
using Application.Contracts.IRepository;
using Application.CQRS.Notification.Commands;
using Application.DTO.Notification;
using Application.Models.Email;
using Application.Services;
using Infrastructure.Email;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [EnableRateLimiting("DefaultPolicy")]
    public class EnhancedNotificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEmailQueueService _emailQueueService;
        // FIX: Change the type to the interface IEmailService
        private readonly IEmailService _enhancedEmailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<EnhancedNotificationController> _logger;
        private readonly IClientApplicationRepository _clientApplicationRepository;

        public EnhancedNotificationController(
            IMediator mediator,
            IEmailQueueService emailQueueService,
            // FIX: Accept the interface type in the constructor
            IEmailService enhancedEmailService,
            IEmailTemplateService emailTemplateService,
            ICacheService cacheService,
            ILogger<EnhancedNotificationController> logger,
            IClientApplicationRepository clientApplicationRepository)
        {
            _mediator = mediator;
            _emailQueueService = emailQueueService;
            _enhancedEmailService = enhancedEmailService;
            _emailTemplateService = emailTemplateService;
            _cacheService = cacheService;
            _logger = logger;
            _clientApplicationRepository = clientApplicationRepository;
        }

        [HttpPost("send")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SendNotification([FromBody] CreateNotificationDto createNotificationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var command = new CreateNotificationCommand { CreateNotificationDto = createNotificationDto };
                var result = await _mediator.Send(command);

                if (result.Success)
                {
                    return Ok(new { message = "Notification sent successfully", id = result.Id });
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification");
                return BadRequest($"Error sending notification: {ex.Message}");
            }
        }

        [HttpPost("send-enhanced")]
        [ProducesResponseType(200)]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SendEnhancedNotification([FromBody] EnhancedNotificationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (request.Metadata == null || !request.Metadata.TryGetValue("ClientApplicationId", out var clientAppIdObj))
                {
                    return BadRequest(new { message = "ClientApplicationId must be provided in Metadata." });
                }

                if (!Guid.TryParse(clientAppIdObj?.ToString(), out var clientAppId))
                {
                    return BadRequest(new { message = "ClientApplicationId is invalid." });
                }

                var clientApp = await _clientApplicationRepository.Get(clientAppId, CancellationToken.None);
                if (clientApp == null)
                {
                    return BadRequest(new { message = "Client application not found." });
                }

                if (request.Metadata == null)
                {
                    request.Metadata = new Dictionary<string, object>();
                }
                request.Metadata["SenderEmail"] = clientApp.SenderEmail;
                request.Metadata["AppPassword"] = clientApp.AppPassword;

                // MODIFIED: Send individual emails to each recipient to ensure privacy
                var successCount = 0;
                var failureCount = 0;
                var errors = new List<string>();

                foreach (var recipient in request.To)
                {
                    var individualEmailMessage = new EnhancedEmailMessage
                    {
                        From = clientApp.SenderEmail,
                        To = new List<string> { recipient }, // Only this recipient
                        Subject = request.Subject,
                        BodyHtml = request.BodyHtml,
                        Priority = request.Priority,
                        ScheduledAt = request.ScheduledAt,
                        EnableTracking = request.EnableTracking,
                        Metadata = request.Metadata
                    };

                    try
                    {
                        if (request.UseQueue)
                        {
                            await _emailQueueService.QueueEmailAsync(individualEmailMessage, (int)request.Priority);
                            successCount++;
                        }
                        else
                        {
                            var success = await _enhancedEmailService.SendEmail(individualEmailMessage);
                            if (success)
                            {
                                successCount++;
                            }
                            else
                            {
                                failureCount++;
                                errors.Add($"Failed to send to {recipient}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        failureCount++;
                        errors.Add($"Error sending to {recipient}: {ex.Message}");
                        _logger.LogError(ex, "Error sending email to {Recipient}", recipient);
                    }
                }

                if (request.UseQueue)
                {
                    return Accepted(new
                    {
                        message = $"Individual emails queued successfully for {successCount} recipients",
                        totalRecipients = request.To.Count,
                        successCount = successCount,
                        failureCount = failureCount,
                        errors = errors
                    });
                }
                else
                {
                    if (successCount > 0)
                    {
                        return Ok(new
                        {
                            message = $"Individual emails sent successfully to {successCount}/{request.To.Count} recipients",
                            totalRecipients = request.To.Count,
                            successCount = successCount,
                            failureCount = failureCount,
                            errors = errors
                        });
                    }
                    else
                    {
                        return StatusCode(500, new
                        {
                            message = "Failed to send emails to all recipients",
                            totalRecipients = request.To.Count,
                            successCount = successCount,
                            failureCount = failureCount,
                            errors = errors
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending enhanced notification");
                return StatusCode(500, new { message = "An error occurred while sending the notification" });
            }
        }

        [HttpPost("send-bulk")]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SendBulkNotification([FromBody] BulkNotificationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                string senderEmail = request.From;
                string appPassword = null;
                if (request.Metadata != null && request.Metadata.TryGetValue("ClientApplicationId", out var clientAppIdObj)
                    && Guid.TryParse(clientAppIdObj?.ToString(), out var clientAppId))
                {
                    var clientApp = await _clientApplicationRepository.Get(clientAppId, CancellationToken.None);
                    if (clientApp != null)
                    {
                        senderEmail = clientApp.SenderEmail;
                        appPassword = clientApp.AppPassword;
                    }
                }

                // MODIFIED: Send individual emails instead of bulk to ensure recipient privacy
                var successCount = 0;
                var failureCount = 0;
                var errors = new List<string>();

                _logger.LogInformation("Sending individual emails to {RecipientCount} recipients to ensure privacy", request.Recipients.Count);

                // Process recipients in batches to avoid overwhelming the system
                var batchSize = request.BatchSize;
                for (int i = 0; i < request.Recipients.Count; i += batchSize)
                {
                    var batch = request.Recipients.Skip(i).Take(batchSize).ToList();

                    foreach (var recipient in batch)
                    {
                        try
                        {
                            // Get personalized data for this recipient if available
                            var personalizedData = request.PersonalizedData?.ContainsKey(recipient) == true
                                ? request.PersonalizedData[recipient]
                                : new Dictionary<string, object>();

                            // Merge global and personalized data
                            var templateData = new Dictionary<string, object>();
                            if (request.GlobalTemplateData != null)
                            {
                                foreach (var kvp in request.GlobalTemplateData)
                                    templateData[kvp.Key] = kvp.Value;
                            }
                            foreach (var kvp in personalizedData)
                                templateData[kvp.Key] = kvp.Value;

                            // Add recipient info to template data
                            templateData["RecipientEmail"] = recipient;

                            var individualEmailMessage = new EnhancedEmailMessage
                            {
                                From = senderEmail,
                                To = new List<string> { recipient }, // Only this recipient
                                Subject = request.Subject,
                                BodyHtml = ProcessTemplate(request.BodyHtml, templateData),
                                BodyText = !string.IsNullOrEmpty(request.BodyText) ? ProcessTemplate(request.BodyText, templateData) : null,
                                Metadata = new Dictionary<string, object>
                                {
                                    ["SenderEmail"] = senderEmail,
                                    ["AppPassword"] = appPassword ?? "",
                                    ["RecipientEmail"] = recipient
                                }
                            };

                            var success = await _enhancedEmailService.SendEmail(individualEmailMessage);
                            if (success)
                            {
                                successCount++;
                                _logger.LogDebug("Successfully sent individual email to {Recipient}", recipient);
                            }
                            else
                            {
                                failureCount++;
                                errors.Add($"Failed to send to {recipient}");
                                _logger.LogWarning("Failed to send individual email to {Recipient}", recipient);
                            }
                        }
                        catch (Exception ex)
                        {
                            failureCount++;
                            errors.Add($"Error sending to {recipient}: {ex.Message}");
                            _logger.LogError(ex, "Error sending individual email to {Recipient}", recipient);
                        }
                    }

                    // Add delay between batches if specified
                    if (i + batchSize < request.Recipients.Count && request.BatchDelayMs > 0)
                    {
                        await Task.Delay(request.BatchDelayMs);
                    }
                }

                return Accepted(new
                {
                    message = $"Individual emails processed successfully for {successCount}/{request.Recipients.Count} recipients",
                    totalRecipients = request.Recipients.Count,
                    successCount = successCount,
                    failureCount = failureCount,
                    errors = errors.Take(10).ToList(), // Limit errors shown
                    note = "Each recipient received an individual email for privacy"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending bulk notification");
                return StatusCode(500, new { message = "An error occurred while sending the bulk notification" });
            }
        }

        [HttpPost("send-templated")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SendTemplatedNotification([FromBody] TemplatedNotificationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                string senderEmail = request.From;
                string appPassword = null;
                if (request.Metadata == null || !request.Metadata.TryGetValue("ClientApplicationId", out var clientAppIdObj))
                {
                    return BadRequest(new { message = "ClientApplicationId must be provided in Metadata." });
                }

                if (!Guid.TryParse(clientAppIdObj?.ToString(), out var clientAppId))
                {
                    return BadRequest(new { message = "ClientApplicationId is invalid." });
                }

                var clientApp = await _clientApplicationRepository.Get(clientAppId, CancellationToken.None);
                if (clientApp == null)
                {
                    return BadRequest(new { message = "Client application not found." });
                }

                senderEmail = clientApp.SenderEmail;
                appPassword = clientApp.AppPassword;

                // MODIFIED: Send individual templated emails to each recipient
                var successCount = 0;
                var failureCount = 0;
                var errors = new List<string>();

                foreach (var recipient in request.Recipients)
                {
                    try
                    {
                        // Create individual template data for this recipient
                        var individualTemplateData = new Dictionary<string, object>(request.TemplateData)
                        {
                            ["RecipientEmail"] = recipient,
                            ["RecipientName"] = ExtractNameFromEmail(recipient)
                        };

                        var success = await _enhancedEmailService.SendTemplatedEmailAsync(
                            request.TemplateName,
                            individualTemplateData,
                            new List<string> { recipient }, // Only this recipient
                            senderEmail,
                            request.Subject,
                            senderEmail,
                            appPassword
                        );

                        if (success)
                        {
                            successCount++;
                        }
                        else
                        {
                            failureCount++;
                            errors.Add($"Failed to send templated email to {recipient}");
                        }
                    }
                    catch (Exception ex)
                    {
                        failureCount++;
                        errors.Add($"Error sending templated email to {recipient}: {ex.Message}");
                        _logger.LogError(ex, "Error sending templated email to {Recipient}", recipient);
                    }
                }

                if (successCount > 0)
                {
                    return Ok(new
                    {
                        message = $"Individual templated emails sent successfully to {successCount}/{request.Recipients.Count} recipients",
                        templateName = request.TemplateName,
                        totalRecipients = request.Recipients.Count,
                        successCount = successCount,
                        failureCount = failureCount,
                        errors = errors,
                        note = "Each recipient received an individual email for privacy"
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        message = "Failed to send templated emails to all recipients",
                        templateName = request.TemplateName,
                        totalRecipients = request.Recipients.Count,
                        successCount = successCount,
                        failureCount = failureCount,
                        errors = errors
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending templated notification");
                return StatusCode(500, new { message = "An error occurred while sending the templated notification" });
            }
        }

        [HttpGet("{id}/track")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> TrackNotification(Guid id)
        {
            try
            {
                var userAgent = Request.Headers["User-Agent"].ToString();
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                _logger.LogInformation("Notification {NotificationId} tracked from IP {IpAddress}", id, ipAddress);

                var pixel = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7");
                return File(pixel, "image/gif");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking notification {NotificationId}", id);
                return BadRequest($"Error tracking notification: {ex.Message}");
            }
        }

        [HttpGet("status/{emailId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmailStatus(Guid emailId)
        {
            try
            {
                var status = await _emailQueueService.GetEmailStatusAsync(emailId);
                return Ok(new { emailId, status = status.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting email status for {EmailId}", emailId);
                return StatusCode(500, new { message = "An error occurred while retrieving email status" });
            }
        }

        [HttpPost("retry/{emailId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RetryFailedEmail(Guid emailId)
        {
            try
            {
                await _emailQueueService.RetryFailedEmailAsync(emailId);
                return Ok(new { message = "Email retry initiated" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrying email {EmailId}", emailId);
                return StatusCode(500, new { message = "An error occurred while retrying the email" });
            }
        }

        [HttpGet("health")]
        [ProducesResponseType(200)]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
        }

        // Helper method to process simple template variables
        private static string ProcessTemplate(string template, Dictionary<string, object> data)
        {
            if (string.IsNullOrEmpty(template) || data == null)
                return template;

            var result = template;
            foreach (var kvp in data)
            {
                result = result.Replace($"{{{kvp.Key}}}", kvp.Value?.ToString() ?? "");
            }
            return result;
        }

        // Helper method to extract name from email
        private static string ExtractNameFromEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return "";

            var atIndex = email.IndexOf('@');
            return atIndex > 0 ? email.Substring(0, atIndex) : email;
        }
    }

    public class EnhancedNotificationRequest
    {
        public required string From { get; set; }
        public required List<string> To { get; set; }
        public List<string>? Cc { get; set; }
        public List<string>? Bcc { get; set; }
        public required string Subject { get; set; }
        public required string BodyHtml { get; set; }
        public EmailPriority Priority { get; set; } = EmailPriority.Normal;
        public DateTime? ScheduledAt { get; set; }
        public bool EnableTracking { get; set; } = true;
        public bool UseQueue { get; set; } = true;
        public Dictionary<string, object>? Metadata { get; set; }
    }

    public class BulkNotificationRequest
    {
        public required List<string> Recipients { get; set; }
        public required string From { get; set; }
        public required string Subject { get; set; }
        public required string BodyHtml { get; set; }
        public string? BodyText { get; set; }
        public int BatchSize { get; set; } = 100;
        public int BatchDelayMs { get; set; } = 1000;
        public Dictionary<string, object>? GlobalTemplateData { get; set; }
        public Dictionary<string, Dictionary<string, object>>? PersonalizedData { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
    }

    public class TemplatedNotificationRequest
    {
        public required string TemplateName { get; set; }
        public required Dictionary<string, object> TemplateData { get; set; }
        public required List<string> Recipients { get; set; }
        public required string From { get; set; }
        public required string Subject { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
    }
}