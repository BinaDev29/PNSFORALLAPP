// File Path: Persistence/Repositories/GenericRepository.cs
using Application.Contracts.IRepository;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<T>(PnsDbContext dbContext) : IGenericRepository<T> where T : BaseDomainEntity
    {
        protected readonly PnsDbContext _dbContext = dbContext;

        public async Task<T> Add(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> Delete(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> Exists(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await Get(id, cancellationToken);
            return entity is not null;
        }

        public async Task<T?> Get(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<bool> Update(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}