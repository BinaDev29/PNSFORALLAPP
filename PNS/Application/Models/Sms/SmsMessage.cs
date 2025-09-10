// File Path: Application/Models/Sms/SmsMessage.cs
namespace Application.Models.Sms
{
    public class SmsMessage
    {
        public required string To { get; set; }
        public required string Body { get; set; }
        public string? TrackingId { get; set; }
        public int MaxRetries { get; set; } = 3;
    }
}