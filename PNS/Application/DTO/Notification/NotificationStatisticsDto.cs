using System;

namespace Application.DTO.Notification
{
    public class NotificationStatisticsDto
    {
        public int TotalRequests { get; set; }
        public int Pending { get; set; }
        public int Sent { get; set; }
        public int Failed { get; set; }
        public int Seen { get; set; }
        public int Scheduled { get; set; }
        public double SuccessRate { get; set; }
    }
}
