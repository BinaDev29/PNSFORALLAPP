// File Path: Application/Models/Email/EmailMessage.cs
namespace Application.Models.Email
{
    public class EmailMessage
    {
        public required List<string> To { get; set; }
        public required string From { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public bool IsHtml { get; set; } = false;
    }
}