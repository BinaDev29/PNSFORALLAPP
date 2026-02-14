// File Path: Application/Models/Email/EnhancedEmailMessage.cs
using System;
using System.Collections.Generic;

namespace Application.Models.Email
{
    public class EnhancedEmailMessage : EmailMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<string>? Cc { get; set; }
        public List<string>? Bcc { get; set; }
        public string? ReplyTo { get; set; }
        public Dictionary<string, string>? Headers { get; set; }
        public List<EmailAttachment>? Attachments { get; set; }
        public EmailTemplate? Template { get; set; }
        public Dictionary<string, object>? TemplateData { get; set; }
        public EmailPriority Priority { get; set; } = EmailPriority.Normal;
        public DateTime? ScheduledAt { get; set; }
        public int MaxRetries { get; set; } = 3;
        public string? TrackingId { get; set; }
        public bool EnableTracking { get; set; } = true;
        public string? Provider { get; set; } // SMTP, SendGrid, etc.
        public Dictionary<string, object>? Metadata { get; set; }
    }

    public class EmailAttachment
    {
        public required string FileName { get; set; }
        public required byte[] Content { get; set; }
        public required string ContentType { get; set; }
        public bool IsInline { get; set; } = false;
        public string? ContentId { get; set; }
    }

    public class EmailTemplate
    {
        public Guid Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string Subject { get; set; } = string.Empty;
        public required string HtmlBody { get; set; } = string.Empty;
        public string? TextBody { get; set; }
        public List<string>? RequiredVariables { get; set; }
    }

    public enum EmailPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Critical = 3
    }
}