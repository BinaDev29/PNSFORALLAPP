// File Path: Application/Contracts/IRepository/IGenericRepository.cs
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface IGenericRepository<T> where T : BaseDomainEntity
    {
        Task<T?> Get(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAll(CancellationToken cancellationToken = default);
        Task<bool> Exists(Guid id, CancellationToken cancellationToken = default);
        Task<T> Add(T entity, CancellationToken cancellationToken = default);
        Task<bool> Update(T entity, CancellationToken cancellationToken = default);
        Task<bool> Delete(T entity, CancellationToken cancellationToken = default);
    }
}