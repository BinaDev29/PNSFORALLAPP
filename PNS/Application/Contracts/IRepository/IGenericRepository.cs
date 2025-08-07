using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> Get(Guid id); // እዚህ ጋር ማስተካከያው ተደርጓል
        Task<IReadOnlyList<T>> GetAll();
        Task<bool> Exists(Guid id);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}