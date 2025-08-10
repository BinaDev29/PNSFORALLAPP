// File Path: Application/Contracts/IEmailService.cs
using Application.Models.Email;

namespace Application.Contracts
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailMessage emailMessage);
    }
}