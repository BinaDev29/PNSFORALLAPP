using Application.DTO.Notification;
using MediatR;
using System;

namespace Application.CQRS.Notification.Queries
{
    public class GetNotificationStatisticsQuery : IRequest<NotificationStatisticsDto>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? ClientApplicationId { get; set; }
        public string? UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
