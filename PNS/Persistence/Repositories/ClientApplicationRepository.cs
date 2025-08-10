// File Path: Persistence/Repositories/ClientApplicationRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;

namespace Persistence.Repositories
{
    public class ClientApplicationRepository(PnsDbContext dbContext) : GenericRepository<ClientApplication>(dbContext), IClientApplicationRepository
    {
    }
}