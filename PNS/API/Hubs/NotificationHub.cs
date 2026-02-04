using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotificationUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveNotificationUpdate", message);
    }
    
    public async Task SendDashboardStats(object stats)
    {
        await Clients.All.SendAsync("ReceiveDashboardStats", stats);
    }
}
