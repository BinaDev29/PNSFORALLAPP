// File Path: Application/Common/Interfaces/IDomainEventService.cs
using Domain.Common;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task PublishAsync(IDomainEvent domainEvent);
    }
}