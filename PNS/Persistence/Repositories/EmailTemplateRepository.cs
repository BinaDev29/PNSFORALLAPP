// File Path: Persistence/Repositories/EmailTemplateRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class EmailTemplateRepository(PnsDbContext dbContext) : GenericRepository<EmailTemplate>(dbContext), IEmailTemplateRepository
    {
        public async Task<IReadOnlyList<EmailTemplate>> GetByUserId(string? userId, bool isAdmin, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.EmailTemplates.AsQueryable();

            if (!isAdmin && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(e => e.CreatedBy == userId);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}