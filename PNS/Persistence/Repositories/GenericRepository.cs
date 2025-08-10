// File Path: Persistence/Repositories/GenericRepository.cs
using Application.Contracts.IRepository;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class GenericRepository<T>(PnsDbContext dbContext) : IGenericRepository<T> where T : BaseDomainEntity
    {
        protected readonly PnsDbContext _dbContext = dbContext;

        public async Task<IReadOnlyList<T>> GetAll(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T?> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        }

        public async Task<T> Add(T entity, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> Update(T entity, CancellationToken cancellationToken)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> Delete(T entity, CancellationToken cancellationToken)
        {
            _dbContext.Remove(entity);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<IReadOnlyList<T>> Find(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().AnyAsync(q => q.Id == id, cancellationToken);
        }
    }
}