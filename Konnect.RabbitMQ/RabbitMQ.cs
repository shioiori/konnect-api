using BuildingBlocks.MessageQueue;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Threading.Channels;

namespace Konnect.RabbitMQ
{
    public class RabbitMQ : IMessagePublisher
    {
        private RabbitMQFactory _factory;
        private ConnectionFactory _connectionFactory;
        private IChannel _channel;
        private IConnection _connection;
        private RabbitMQConsumerData _consumer;
        public RabbitMQ(RabbitMQFactory factory)
        {
            _factory = factory;
        }

        public async Task Start()
        {
            IConnectionFactory connectionFactory = _factory.Build();
            _connection = await _connectionFactory.CreateConnectionAsync();
        }

        public void Publish<T>(T message)
        {
            byte[] messagebuffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _channel.BasicPublishAsync(_consumer.ExchangeName, _consumer.RoutingKey, messagebuffer);
        }

        public async Task RegisterChannelAsync(string exchangeName, string queueName, string exchangeType, string routingKey)
        {
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(exchangeName, exchangeType);
            await _channel.QueueDeclareAsync(queueName, false, false, false, null);
            await _channel.QueueBindAsync(queueName, exchangeName, routingKey, null);
        }

        public void DeclareCurrentChannel(string exchangeName, string routingKey)
        {
            _consumer = new RabbitMQConsumerData(exchangeName, routingKey);
        }
    }
}
