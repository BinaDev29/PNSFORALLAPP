// File Path: Application/Models/Email/EmailMessage.cs
using System.Collections.Generic;

namespace Application.Models.Email
{
    public class EmailMessage
    {
        public required string From { get; set; }
        public required List<string> To { get; set; }
        public required string Subject { get; set; }
        public required string BodyHtml { get; set; }
        public string? BodyText { get; set; }
    }
}