using Application.Contracts;
using API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace API.Services;

public class DashboardHubService : IDashboardHubService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public DashboardHubService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotificationUpdate(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotificationUpdate", message);
    }

    public async Task SendDashboardStats(object stats)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveDashboardStats", stats);
    }
}
