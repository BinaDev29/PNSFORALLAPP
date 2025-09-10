// File Path: Application/Contracts/ISmsService.cs
using Application.Models.Sms;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(SmsMessage smsMessage);
    }
}