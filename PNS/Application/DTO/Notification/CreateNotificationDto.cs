namespace Application.DTO.Notification
{
    public class CreateNotificationDto
    {
        public Guid ClientApplicationId { get; set; }
        public Guid NotificationTypeId { get; set; }
        public required string Recipient { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
    }
}