// File Path: Application/Contracts/IServices/IPushNotificationService.cs
using Domain.Models;
using System.Threading.Tasks;

namespace Application.Contracts.IServices
{
    public interface IPushNotificationService
    {
        Task SendNotificationAsync(Notification notification);
    }
}