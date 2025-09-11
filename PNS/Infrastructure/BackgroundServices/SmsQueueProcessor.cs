// File Path: Infrastructure/BackgroundServices/SmsQueueProcessor.cs
using Application.Contracts;
using Infrastructure.Sms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.BackgroundServices
{
    public class SmsQueueProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SmsQueueProcessor> _logger;
        private readonly TimeSpan _processingInterval = TimeSpan.FromSeconds(5);

        public SmsQueueProcessor(IServiceProvider serviceProvider, ILogger<SmsQueueProcessor> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SMS Queue Processor started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var smsQueueService = scope.ServiceProvider.GetRequiredService<ISmsQueueService>();
                    var smsService = scope.ServiceProvider.GetRequiredService<ISmsService>();

                    var smsMessage = await smsQueueService.DequeueAsync();
                    if (smsMessage != null)
                    {
                        _logger.LogInformation("Processing SMS message for {To}", smsMessage.To);

                        var success = await smsService.SendSmsAsync(smsMessage);
                        if (success)
                        {
                            _logger.LogInformation("SMS message processed successfully for {To}", smsMessage.To);
                        }
                        else
                        {
                            _logger.LogWarning("Failed to process SMS message for {To}, RetryCount: {RetryCount}",
                                smsMessage.To, smsMessage.RetryCount);

                            // Re-queue if retries available
                            if (smsMessage.RetryCount < smsMessage.MaxRetries)
                            {
                                await Task.Delay(TimeSpan.FromMinutes(Math.Pow(2, smsMessage.RetryCount)), stoppingToken);
                                await smsQueueService.EnqueueSmsAsync(smsMessage);
                            }
                            else
                            {
                                _logger.LogError("SMS message failed permanently for {To} after {MaxRetries} retries",
                                    smsMessage.To, smsMessage.MaxRetries);
                            }
                        }
                    }
                    else
                    {
                        // No messages in queue, wait before checking again
                        await Task.Delay(_processingInterval, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing SMS queue");
                    await Task.Delay(_processingInterval, stoppingToken);
                }
            }

            _logger.LogInformation("SMS Queue Processor stopped");
        }
    }
}