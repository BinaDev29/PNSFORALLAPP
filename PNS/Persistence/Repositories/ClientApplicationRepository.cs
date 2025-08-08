// File Path: Persistence/Repositories/ClientApplicationRepository.cs
using Application.Contracts.IRepository;
using Domain.Models;
using Persistence;
using Persistence.Repositories; // 🟢 ይህን መስመር መጨመር አስፈላጊ ነው

// ከ`GenericRepository` በትክክል መውረስ
public class ClientApplicationRepository(PnsDbContext dbContext)
    : GenericRepository<ClientApplication>(dbContext), IClientApplicationRepository
{
    // ለ`ClientApplication` ልዩ ተግባራት ካሉ እዚህ ይጨመራሉ
}