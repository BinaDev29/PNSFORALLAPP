// File Path: Application/Contracts/IRepository/ISmsTemplateRepository.cs

using Domain.Models;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    // Extends the generic repository interface for specific SMS template operations
    public interface ISmsTemplateRepository : IGenericRepository<SmsTemplate>
    {
        // Add SMS-template-specific methods
        Task<SmsTemplate?> GetByNameAsync(string templateName);
        Task<List<SmsTemplate>> GetActiveTemplatesAsync();
        Task<IReadOnlyList<SmsTemplate>> GetByUserId(string? userId, bool isAdmin, CancellationToken cancellationToken = default);
    }
}