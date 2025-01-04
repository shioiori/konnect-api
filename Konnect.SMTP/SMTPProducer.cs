using BuildingBlocks.MessageQueue;
using Konnect.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konnect.SMTP
{
    public record SMTPPublishData(string ExchangeName, string RoutingKey);
    public class SMTPProducer : RabbitMQService, IMessagePublisher
    {
        private SMTPPublishData _data;
        public SMTPProducer(RabbitMQFactory factory) : base(factory) 
        {
        }

        public void DeclareProducer(string exchangeName, string routingKey)
        {
            _data = new SMTPPublishData(exchangeName, routingKey);
        }

        public void Publish<T>(T message)
        {
            PublishInternal(message);
        }

        private int _retryTime = 0;
        private async Task PublishInternal<T>(T message)
        {
            try
            {
                byte[] messageBuffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                _channel.BasicPublishAsync(_data.ExchangeName, _data.RoutingKey, messageBuffer);
            }
            catch (Exception ex)
            {
                if (_retryTime == 0)
                {
                    _retryTime++;
                    await CreateChannelAsync();
                    PublishInternal(message);
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                _retryTime = 0;
            }
        }
    }
}
