namespace Application.Contracts;

public interface IDashboardHubService
{
    Task SendNotificationUpdate(string message);
    Task SendDashboardStats(object stats);
}
