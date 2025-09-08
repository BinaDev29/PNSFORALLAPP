using Application.Common.Interfaces;
using Application.Models.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.BackgroundServices
{
    public class EmailQueueProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailQueueProcessor> _logger;
        private readonly ConcurrentQueue<QueuedEmail> _emailQueue;
        private readonly SemaphoreSlim _semaphore;

        public EmailQueueProcessor(IServiceProvider serviceProvider, ILogger<EmailQueueProcessor> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _emailQueue = new ConcurrentQueue<QueuedEmail>();
            _semaphore = new SemaphoreSlim(5, 5); // Process max 5 emails concurrently
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email Queue Processor started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_emailQueue.TryDequeue(out var queuedEmail))
                    {
                        await _semaphore.WaitAsync(stoppingToken);

                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                await ProcessEmailAsync(queuedEmail);
                            }
                            finally
                            {
                                _semaphore.Release();
                            }
                        }, stoppingToken);
                    }
                    else
                    {
                        await Task.Delay(1000, stoppingToken); // Wait 1 second if no emails in queue
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in email queue processor");
                }
            }

            _logger.LogInformation("Email Queue Processor stopped");
        }

        private async Task ProcessEmailAsync(QueuedEmail queuedEmail)
        {
            using var scope = _serviceProvider.CreateScope();
            var emailService = scope.ServiceProvider.GetRequiredService<Infrastructure.Email.EnhancedEmailService>();

            try
            {
                _logger.LogInformation("Processing email {EmailId}", queuedEmail.Email.Id);

                var success = await emailService.SendEnhancedEmailAsync(queuedEmail.Email);

                if (success)
                {
                    _logger.LogInformation("Email {EmailId} sent successfully", queuedEmail.Email.Id);
                }
                else
                {
                    _logger.LogWarning("Failed to send email {EmailId}, attempt {Attempt}",
                        queuedEmail.Email.Id, queuedEmail.AttemptCount);

                    if (queuedEmail.AttemptCount < queuedEmail.Email.MaxRetries)
                    {
                        queuedEmail.AttemptCount++;
                        queuedEmail.NextAttempt = DateTime.UtcNow.AddMinutes(Math.Pow(2, queuedEmail.AttemptCount)); // Exponential backoff

                        // Re-queue for retry
                        _logger.LogInformation("Re-queuing email {EmailId} for attempt {Attempt}", queuedEmail.Email.Id, queuedEmail.AttemptCount);
                        await Task.Delay(TimeSpan.FromMinutes(1)); // Wait before re-queuing
                        _emailQueue.Enqueue(queuedEmail);
                    }
                    else
                    {
                        _logger.LogError("Email {EmailId} failed permanently after {MaxRetries} attempts",
                            queuedEmail.Email.Id, queuedEmail.Email.MaxRetries);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception processing email {EmailId}", queuedEmail.Email.Id);
            }
        }

        public void QueueEmail(EnhancedEmailMessage email, int priority = 0)
        {
            var queuedEmail = new QueuedEmail
            {
                Email = email,
                Priority = priority,
                QueuedAt = DateTime.UtcNow,
                AttemptCount = 0,
                NextAttempt = DateTime.UtcNow
            };

            _emailQueue.Enqueue(queuedEmail);
            _logger.LogInformation("Email {EmailId} queued with priority {Priority}", email.Id, priority);
        }
    }

    public class QueuedEmail
    {
        public required EnhancedEmailMessage Email { get; set; }
        public int Priority { get; set; }
        public DateTime QueuedAt { get; set; }
        public int AttemptCount { get; set; }
        public DateTime NextAttempt { get; set; }
    }

    public class EmailQueueService : IEmailQueueService
    {
        private readonly EmailQueueProcessor _processor;
        private readonly ILogger<EmailQueueService> _logger;

        public EmailQueueService(EmailQueueProcessor processor, ILogger<EmailQueueService> logger)
        {
            _processor = processor;
            _logger = logger;
        }

        public Task QueueEmailAsync(EnhancedEmailMessage emailMessage, int priority = 0)
        {
            _processor.QueueEmail(emailMessage, priority);
            return Task.CompletedTask;
        }

        public Task QueueBulkEmailAsync(BulkEmailMessage bulkEmailMessage, int priority = 0)
        {
            // Convert bulk message to individual enhanced messages
            foreach (var recipient in bulkEmailMessage.Recipients)
            {
                var enhancedEmail = new EnhancedEmailMessage
                {
                    From = bulkEmailMessage.From,
                    To = new List<string> { recipient },
                    Subject = bulkEmailMessage.Subject,
                    BodyHtml = bulkEmailMessage.BodyHtml,
                    Priority = (EmailPriority)priority
                };

                _processor.QueueEmail(enhancedEmail, priority);
            }

            return Task.CompletedTask;
        }

        public Task<EmailQueueStatus> GetEmailStatusAsync(Guid emailId)
        {
            // This would typically query a database for email status
            // For now, return a default status
            return Task.FromResult(EmailQueueStatus.Queued);
        }

        public Task RetryFailedEmailAsync(Guid emailId)
        {
            // This would typically find the failed email and re-queue it
            _logger.LogInformation("Retrying failed email {EmailId}", emailId);
            return Task.CompletedTask;
        }
    }
}