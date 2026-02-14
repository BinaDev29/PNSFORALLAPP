// File Path: Domain/Models/SmsMessage.cs
using System;

namespace Domain.Models
{
    public class SmsMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string To { get; set; }
        public string? From { get; set; }
        public required string Body { get; set; }
        public string? TrackingId { get; set; }
        public int MaxRetries { get; set; } = 3;
        public int RetryCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SentAt { get; set; }
        public string? Status { get; set; }
        public string? ErrorMessage { get; set; }
    }
}