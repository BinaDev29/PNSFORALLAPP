using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IMessageQueueService
    {
        Task PublishMessageAsync<T>(string queueName, T message);
        Task SubscribeAsync<T>(string queueName, System.Func<T, Task> handler);
    }
}
