// File Path: Persistence/Repositories/EmailTemplateRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;

namespace Persistence.Repositories
{
    public class EmailTemplateRepository(PnsDbContext dbContext) : GenericRepository<EmailTemplate>(dbContext), IEmailTemplateRepository
    {
    }
}