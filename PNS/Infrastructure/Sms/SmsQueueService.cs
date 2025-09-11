// File Path: Infrastructure/Sms/SmsQueueService.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Sms
{
    public class SmsQueueService : ISmsQueueService
    {
        private readonly ConcurrentQueue<SmsMessage> _smsQueue;
        private readonly ILogger<SmsQueueService> _logger;
        private readonly SemaphoreSlim _semaphore;

        public SmsQueueService(ILogger<SmsQueueService> logger)
        {
            _smsQueue = new ConcurrentQueue<SmsMessage>();
            _logger = logger;
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task EnqueueSmsAsync(SmsMessage smsMessage)
        {
            await _semaphore.WaitAsync();
            try
            {
                _smsQueue.Enqueue(smsMessage);
                _logger.LogInformation("SMS message queued for {To}, Queue size: {QueueSize}",
                    smsMessage.To, _smsQueue.Count);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<SmsMessage?> DequeueAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_smsQueue.TryDequeue(out var smsMessage))
                {
                    _logger.LogInformation("SMS message dequeued for {To}, Remaining queue size: {QueueSize}",
                        smsMessage.To, _smsQueue.Count);
                    return smsMessage;
                }
                return null;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public int GetQueueSize()
        {
            return _smsQueue.Count;
        }
    }
}