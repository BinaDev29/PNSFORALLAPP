// File Path: Persistence/Repositories/PriorityRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;

namespace Persistence.Repositories
{
    public class PriorityRepository(PnsDbContext dbContext) : GenericRepository<Priority>(dbContext), IPriorityRepository
    {
    }
}