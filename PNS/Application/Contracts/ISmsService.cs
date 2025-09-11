// File Path: Application/Contracts/ISmsService.cs
using Domain.Models;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(SmsMessage smsMessage);
    }
}