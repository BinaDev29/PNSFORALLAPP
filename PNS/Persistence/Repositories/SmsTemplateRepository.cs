// File Path: Persistence/Repositories/SmsTemplateRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class SmsTemplateRepository : GenericRepository<SmsTemplate>, ISmsTemplateRepository
    {
        private new readonly PnsDbContext _dbContext;

        public SmsTemplateRepository(PnsDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SmsTemplate?> GetByNameAsync(string templateName)
        {
            return await _dbContext.SmsTemplates
                .FirstOrDefaultAsync(t => t.Name == templateName);
        }

        public async Task<List<SmsTemplate>> GetActiveTemplatesAsync()
        {
            return await _dbContext.SmsTemplates
                .Where(t => !t.IsDeleted)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<SmsTemplate>> GetByUserId(string? userId, bool isAdmin, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.SmsTemplates.AsQueryable();

            if (!isAdmin && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(s => s.CreatedBy == userId);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}