// File Path: Application/Models/Email/BulkEmailMessage.cs
using System;
using System.Collections.Generic;

namespace Application.Models.Email
{
    public class BulkEmailMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required List<string> Recipients { get; set; }
        public required string From { get; set; }
        public required string Subject { get; set; }
        public required string BodyHtml { get; set; }
        public string? BodyText { get; set; }
        public EmailTemplate? Template { get; set; }
        public Dictionary<string, object>? GlobalTemplateData { get; set; }
        public Dictionary<string, Dictionary<string, object>>? PersonalizedData { get; set; }
        public int BatchSize { get; set; } = 100;
        public TimeSpan BatchDelay { get; set; } = TimeSpan.FromSeconds(1);
        public int MaxRetries { get; set; } = 3;
    }
}