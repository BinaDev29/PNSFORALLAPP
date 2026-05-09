using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IPushNotificationService
    {
        Task<bool> SendPushAsync(string token, string title, string body, Dictionary<string, string>? data = null);
        Task<int> SendBatchPushAsync(List<string> tokens, string title, string body, Dictionary<string, string>? data = null);
    }
}
