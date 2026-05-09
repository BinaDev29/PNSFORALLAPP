using Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RabbitMqService : IMessageQueueService, IDisposable
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly ConnectionFactory _factory;
        private readonly ILogger<RabbitMqService> _logger;

        public RabbitMqService(IConfiguration configuration, ILogger<RabbitMqService> logger)
        {
            _logger = logger;
            _factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"] ?? "localhost",
                UserName = configuration["RabbitMQ:UserName"] ?? "guest",
                Password = configuration["RabbitMQ:Password"] ?? "guest"
            };
        }

        private async Task EnsureConnectedAsync()
        {
            if (_channel != null) return;

            try
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to RabbitMQ.");
            }
        }

        public async Task PublishMessageAsync<T>(string queueName, T message)
        {
            await EnsureConnectedAsync();
            if (_channel == null) return;

            await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await _channel.BasicPublishAsync(exchange: "", routingKey: queueName, mandatory: false, basicProperties: properties, body: body);
        }

        public async Task SubscribeAsync<T>(string queueName, Func<T, Task> handler)
        {
            await EnsureConnectedAsync();
            if (_channel == null) return;

            await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));
                if (message != null)
                {
                    await handler(message);
                }
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
