// File Path: Application/Contracts/ISmsQueueService.cs
using Domain.Models;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ISmsQueueService
    {
        Task EnqueueSmsAsync(SmsMessage smsMessage);
        Task<SmsMessage?> DequeueAsync();
        int GetQueueSize();
    }
}