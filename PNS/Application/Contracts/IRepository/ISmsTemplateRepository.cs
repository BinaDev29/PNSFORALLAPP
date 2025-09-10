// File Path: Application/Contracts/IRepository/ISmsTemplateRepository.cs

using Domain.Models;

namespace Application.Contracts.IRepository
{
    // Extends the generic repository interface for specific SMS template operations
    public interface ISmsTemplateRepository : IGenericRepository<SmsTemplate>
    {
        // Add any SMS-template-specific methods here if needed
        // For example: Task<SmsTemplate> GetByTemplateName(string name);
    }
}