using BuildingBlocks.MessageQueue;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Threading.Channels;

namespace Konnect.RabbitMQ
{
    public interface IRabbitMQService
    {
        Task StartConnectionAsync(CancellationToken cancellationToken = default);
        Task CloseConnectionAsync(bool dispose = false, CancellationToken cancellationToken = default);
        Task<IChannel> CreateChannelAsync();
        Task CloseChannelAsync();
        Task RegisterChannelAsync(string? exchangeName, string? queueName, string? exchangeType, string? routingKey);
    }
    public class RabbitMQService : IRabbitMQService
    {
        private RabbitMQFactory _factory;
        private ConnectionFactory _connectionFactory;
        protected IChannel _channel;
        protected IConnection _connection;

        public RabbitMQService()
        {
            _factory = new RabbitMQFactory();
        }
        public RabbitMQService(RabbitMQFactory factory)
        {
            _factory = factory;
        }

        public async Task StartConnectionAsync(CancellationToken cancellationToken = default)
        {
            IConnectionFactory connectionFactory = _factory.Build();
            _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        }

        public async Task CloseConnectionAsync(bool dispose = false, CancellationToken cancellationToken = default)
        {
            await _connection.CloseAsync(cancellationToken);
            if (dispose)
            {
                _connection.Dispose();
            }
        }

        public async Task<IChannel> CreateChannelAsync()
        {
            _channel = await _connection.CreateChannelAsync();
            return _channel;
        }

        public async Task RegisterChannelAsync(string? exchangeName, string? queueName, string? exchangeType, string? routingKey)
        {
            if (!string.IsNullOrEmpty(exchangeName))
            {
                await _channel.ExchangeDeclareAsync(exchangeName, string.IsNullOrEmpty(exchangeType) ? ExchangeType.Direct : exchangeType);
            }
            if (!string.IsNullOrEmpty(queueName))
            {
                await _channel.QueueDeclareAsync(queueName, false, false, false, null);
            }
            if (!string.IsNullOrEmpty(routingKey))
            {
                await _channel.QueueBindAsync(queueName, exchangeName, routingKey, null);
            }
        }

        public async Task CloseChannelAsync()
        {
            await _channel.CloseAsync();
        }
    }
}
