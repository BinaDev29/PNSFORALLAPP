// File Path: Application/Contracts/IRepository/IGenericRepository.cs
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        // Get method should return a nullable Task<T> if the item might not be found.
        Task<T?> Get(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAll(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetWhere(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> Add(T entity, CancellationToken cancellationToken = default);
        Task<int> Count(CancellationToken cancellationToken = default);

        // Update and Delete should return the updated/deleted entity as Task<T>.
        Task<T> Update(T entity, CancellationToken cancellationToken = default);
        Task<T> Delete(T entity, CancellationToken cancellationToken = default);
    }
}