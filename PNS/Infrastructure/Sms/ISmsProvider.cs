// File Path: Infrastructure/Sms/ISmsProvider.cs
using Domain.Models;
using System.Threading.Tasks;

namespace Infrastructure.Sms
{
    public interface ISmsProvider
    {
        string Name { get; }
        int Priority { get; }
        Task<SmsSendResult> SendSmsAsync(SmsMessage smsMessage);
    }
}