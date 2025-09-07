// File Path: API/Controllers/EnhancedNotificationController.cs
using Application.Common.Interfaces;
using Application.CQRS.Notification.Commands;
using Application.DTO.Notification;
using Application.Models.Email;
using Application.Services;
using Infrastructure.Email;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [EnableRateLimiting("DefaultPolicy")]
    public class EnhancedNotificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEmailQueueService _emailQueueService;
        private readonly EnhancedEmailService _enhancedEmailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<EnhancedNotificationController> _logger;

        public EnhancedNotificationController(
            IMediator mediator,
            IEmailQueueService emailQueueService,
            EnhancedEmailService enhancedEmailService,
            IEmailTemplateService emailTemplateService,
            ICacheService cacheService,
            ILogger<EnhancedNotificationController> logger)
        {
            _mediator = mediator;
            _emailQueueService = emailQueueService;
            _enhancedEmailService = enhancedEmailService;
            _emailTemplateService = emailTemplateService;
            _cacheService = cacheService;
            _logger = logger;
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

                var emailMessage = new EnhancedEmailMessage
                {
                    From = request.From,
                    To = request.To,
                    Subject = request.Subject,
                    BodyHtml = request.BodyHtml,
                    Cc = request.Cc,
                    Bcc = request.Bcc,
                    Priority = request.Priority,
                    ScheduledAt = request.ScheduledAt,
                    EnableTracking = request.EnableTracking,
                    Metadata = request.Metadata
                };

                if (request.UseQueue)
                {
                    await _emailQueueService.QueueEmailAsync(emailMessage, (int)request.Priority);
                    return Accepted(new { message = "Email queued successfully", id = emailMessage.Id });
                }
                else
                {
                    var success = await _enhancedEmailService.SendEnhancedEmailAsync(emailMessage);
                    if (success)
                    {
                        return Ok(new { message = "Email sent successfully", id = emailMessage.Id });
                    }
                    else
                    {
                        return StatusCode(500, new { message = "Failed to send email" });
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

                var bulkEmailMessage = new BulkEmailMessage
                {
                    Recipients = request.Recipients,
                    From = request.From,
                    Subject = request.Subject,
                    BodyHtml = request.BodyHtml,
                    BodyText = request.BodyText,
                    BatchSize = request.BatchSize,
                    BatchDelay = TimeSpan.FromMilliseconds(request.BatchDelayMs)
                };

                await _emailQueueService.QueueBulkEmailAsync(bulkEmailMessage);
                return Accepted(new { message = "Bulk email queued successfully", id = bulkEmailMessage.Id });
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

                var success = await _enhancedEmailService.SendTemplatedEmailAsync(
                    request.TemplateName,
                    request.TemplateData,
                    request.Recipients,
                    request.From,
                    request.Subject
                );

                if (success)
                {
                    return Ok(new { message = "Templated email sent successfully" });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to send templated email" });
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
                // This endpoint is called when the tracking pixel is loaded
                var userAgent = Request.Headers["User-Agent"].ToString();
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                // Update notification as seen (would typically update database)
                _logger.LogInformation("Notification {NotificationId} tracked from IP {IpAddress}", id, ipAddress);

                // Return a 1x1 transparent pixel
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
    }

    public class TemplatedNotificationRequest
    {
        public required string TemplateName { get; set; }
        public required Dictionary<string, object> TemplateData { get; set; }
        public required List<string> Recipients { get; set; }
        public required string From { get; set; }
        public required string Subject { get; set; }
    }
}