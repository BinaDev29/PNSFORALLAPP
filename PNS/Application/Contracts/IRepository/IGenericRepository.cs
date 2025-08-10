// File Path: Application/Contracts/IRepository/IGenericRepository.cs
using Domain.Common;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface IGenericRepository<T> where T : BaseDomainEntity
    {
        Task<IReadOnlyList<T>> GetAll(CancellationToken cancellationToken);
        Task<T?> Get(Guid id, CancellationToken cancellationToken);
        Task<T> Add(T entity, CancellationToken cancellationToken);
        Task<bool> Update(T entity, CancellationToken cancellationToken);
        Task<bool> Delete(T entity, CancellationToken cancellationToken);
        Task<IReadOnlyList<T>> Find(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<bool> Exists(Guid id, CancellationToken cancellationToken);
    }
}