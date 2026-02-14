// File Path: Application/Models/Email/EmailMessage.cs
using System.Collections.Generic;

namespace Application.Models.Email
{
    public class EmailMessage
    {
        public required string From { get; set; } = string.Empty;
        public required List<string> To { get; set; } = new();
        public required string Subject { get; set; } = string.Empty;
        public required string BodyHtml { get; set; } = string.Empty;
        public string? BodyText { get; set; }
    }
}