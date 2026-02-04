// File Path: Application/Contracts/IRepository/IEmailTemplateRepository.cs
using Domain.Models;

namespace Application.Contracts.IRepository
{
    public interface IEmailTemplateRepository : IGenericRepository<EmailTemplate>
    {
        Task<IReadOnlyList<EmailTemplate>> GetByUserId(string? userId, bool isAdmin, CancellationToken cancellationToken = default);
    }
}