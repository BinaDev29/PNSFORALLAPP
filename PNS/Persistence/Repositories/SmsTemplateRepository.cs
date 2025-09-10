// File Path: Persistence/Repositories/SmsTemplateRepository.cs

using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;

namespace Persistence.Repositories
{
    // Implements the ISmsTemplateRepository interface
    public class SmsTemplateRepository : GenericRepository<SmsTemplate>, ISmsTemplateRepository
    {
        // The base class handles most of the basic CRUD operations
        public SmsTemplateRepository(PnsDbContext dbContext) : base(dbContext)
        {
        }
    }
}