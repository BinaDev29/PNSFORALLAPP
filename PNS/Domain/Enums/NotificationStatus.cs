// File Path: Domain/Enums/NotificationStatus.cs
namespace Domain.Enums
{
    public enum NotificationStatus
    {
        Pending = 0,
        Scheduled = 1,
        Sending = 2,
        Sent = 3,
        Seen = 4,
        Failed = 5,
        Cancelled = 6
    }
}