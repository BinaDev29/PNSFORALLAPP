// File Path: Application/Contracts/IRepository/INotificationRepository.cs
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        // ለ`Notification` ልዩ የሆኑ ዘዴዎች ካስፈለጉ እዚህ ይጨመራሉ
    }
}