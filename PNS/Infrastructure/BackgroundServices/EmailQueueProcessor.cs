// File Path: Infrastructure/BackgroundServices/EmailQueueProcessor.cs
using Application.Common.Interfaces;
using Application.Models.Email;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.BackgroundServices
{
    public class EmailQueueProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailQueueProcessor> _logger;
        private readonly ConcurrentDictionary<Guid, QueuedEmail> _emailQueue;
        private readonly SemaphoreSlim _semaphore;

        public EmailQueueProcessor(IServiceProvider serviceProvider, ILogger<EmailQueueProcessor> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _emailQueue = new ConcurrentDictionary<Guid, QueuedEmail>();
            _semaphore = new SemaphoreSlim(5, 5);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email Queue Processor started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var nextEmail = _emailQueue.Values
                        .OrderByDescending(e => e.Priority)
                        .ThenBy(e => e.NextAttempt)
                        .FirstOrDefault(e => e.NextAttempt <= DateTime.UtcNow);

                    if (nextEmail != null)
                    {
                        if (_emailQueue.TryRemove(nextEmail.Email.Id, out var queuedEmail))
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
                    }
                    else
                    {
                        await Task.Delay(1000, stoppingToken);
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
            var emailService = scope.ServiceProvider.GetRequiredService<Application.Contracts.IEmailService>();

            try
            {
                _logger.LogInformation("Processing email {EmailId}", queuedEmail.Email.Id);

                // FIX: The method call is now corrected to use the public IEmailService method.
                var success = await emailService.SendEmail(queuedEmail.Email);

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
                        queuedEmail.NextAttempt = DateTime.UtcNow.AddMinutes(Math.Pow(2, queuedEmail.AttemptCount));

                        _logger.LogInformation("Re-queuing email {EmailId} for attempt {Attempt} at {NextAttempt}", queuedEmail.Email.Id, queuedEmail.AttemptCount, queuedEmail.NextAttempt);
                        _emailQueue.TryAdd(queuedEmail.Email.Id, queuedEmail);
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

            if (!_emailQueue.TryAdd(email.Id, queuedEmail))
            {
                _logger.LogWarning("Email {EmailId} is already in the queue. Skipping.", email.Id);
            }
            else
            {
                _logger.LogInformation("Email {EmailId} queued with priority {Priority}", email.Id, priority);
            }
        }
    }
}