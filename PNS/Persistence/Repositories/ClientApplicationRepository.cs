// File Path: Persistence/Repositories/ClientApplicationRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class ClientApplicationRepository(PnsDbContext dbContext) : GenericRepository<ClientApplication>(dbContext), IClientApplicationRepository
    {
        public async Task<IReadOnlyList<ClientApplication>> GetByUserId(string? userId, bool isAdmin, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.ClientApplications.AsQueryable();

            if (!isAdmin && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(c => c.CreatedBy == userId);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}