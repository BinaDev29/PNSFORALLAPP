// File Path: Application/Contracts/IRepository/IClientApplicationRepository.cs
using Domain.Models;
using Application.Contracts.IRepository;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface IClientApplicationRepository : IGenericRepository<ClientApplication>
    {
        // ለ`ClientApplication` ብቻ የሚያገለግሉ ተጨማሪ ዘዴዎች ካሉ እዚህ መጻፍ ትችላለህ
    }
}