using Application.DTO.Webhook;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IWebhookService
    {
        Task SendWebhookAsync(string url, string secret, WebhookPayload payload);
    }
}
