// File Path: Application/Contracts/IEmailService.cs
using Application.Models.Email;
using System;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailMessage emailMessage, Guid notificationId, string appemail, string apppassword);
    }
}