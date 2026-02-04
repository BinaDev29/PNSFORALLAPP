// File Path: Persistence/Repositories/GenericRepository.cs
using Application.Contracts.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<T>(PnsDbContext dbContext) : IGenericRepository<T> where T : class
    {
        protected readonly PnsDbContext _dbContext = dbContext;
        protected readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public async Task<T> Add(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<int> Count(CancellationToken cancellationToken)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public async Task<T?> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAll(CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetWhere(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(predicate).AsNoTracking().ToListAsync(cancellationToken);
        }

        // Fix: The Update method now returns the entity
        public async Task<T> Update(T entity, CancellationToken cancellationToken)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask; // Or use another async operation if you have one.
            return entity;
        }

        // Fix: The Delete method now returns the entity
        public async Task<T> Delete(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask; // Or use another async operation if you have one.
            return entity;
        }
    }
}