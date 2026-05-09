using System;

namespace Application.DTO.Analytics
{
    public class NotificationStatsDto
    {
        public int TotalSent { get; set; }
        public int TotalSeen { get; set; }
        public int TotalFailed { get; set; }
        public double OpenRate => TotalSent > 0 ? (double)TotalSeen / TotalSent * 100 : 0;
        public int TotalSms { get; set; }
        public int TotalEmail { get; set; }
    }
}
