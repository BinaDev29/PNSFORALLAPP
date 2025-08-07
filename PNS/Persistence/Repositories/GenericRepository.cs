// File Path: Persistence/Repositories/GenericRepository.cs
using Application.Contracts.IRepository;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistence; // እዚህ ጋር ማስተካከያው ተደርጓል
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseDomainEntity
    {
        protected readonly PnsDbContext _context;

        public GenericRepository(PnsDbContext context)
        {
            _context = context;
        }

        public async Task<T?> Get(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.Set<T>().AnyAsync(q => q.Id == id);
        }

        public async Task<T> Add(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}