// File Path: Application/Models/Sms/EnhancedSmsMessage.cs
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.Models.Sms
{
    public class EnhancedSmsMessage : SmsMessage
    {
        public string? TemplateId { get; set; }
        public Dictionary<string, string>? TemplateParameters { get; set; }
        public int Priority { get; set; } = 1;
        public DateTime? ScheduledAt { get; set; }
        public string? ApplicationId { get; set; }
        public string? UserId { get; set; }
        public List<string>? Tags { get; set; }
        public string? Campaign { get; set; }

        public EnhancedSmsMessage()
        {
            TemplateParameters = new Dictionary<string, string>();
            Tags = new List<string>();
        }
    }
}