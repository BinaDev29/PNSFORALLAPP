// File Path: Application/Models/Email/EmailMessage.cs
using System.Collections.Generic;

namespace Application.Models.Email
{
    public class EmailMessage
    {
        public required List<string> To { get; set; }
        public required string From { get; set; }
        public required string Subject { get; set; }
        public required string BodyHtml { get; set; } // ⭐ Bodyን ወደ BodyHtml ቀይረው ⭐
    }
}