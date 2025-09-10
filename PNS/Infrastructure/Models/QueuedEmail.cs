// File Path: Infrastructure/Models/QueuedEmail.cs
using Application.Models.Email;
using System;

namespace Infrastructure.Models
{
    public class QueuedEmail
    {
        public required EnhancedEmailMessage Email { get; set; }
        public int Priority { get; set; }
        public DateTime QueuedAt { get; set; }
        public int AttemptCount { get; set; }
        public DateTime NextAttempt { get; set; }
    }
}