// File Path: Application/Contracts/IRepository/IClientApplicationRepository.cs
using Domain.Models;

namespace Application.Contracts.IRepository
{
    public interface IClientApplicationRepository : IGenericRepository<ClientApplication>
    {
        Task<IReadOnlyList<ClientApplication>> GetByUserId(string? userId, bool isAdmin, CancellationToken cancellationToken = default);
    }
}