using BuildingBlocks.MessageQueue;
using Konnect.RabbitMQ;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Konnect.RabbitMQ;
using System.Net.Mail;
using Newtonsoft.Json;
using MimeKit;

namespace Konnect.SMTP
{
    public class STMPConsumer : RabbitMQService, IMessageConsumer
    {
        private string _queueName;
        private STMPHandler _handler;
        public STMPConsumer(STMPHandler stmqHandler, RabbitMQFactory factory) : base(factory)
        {
            _handler = stmqHandler;
        }
        public void DeclareConsumer(string queueName)
        {
            _queueName = queueName;
        }

        public void Consume()
        {
            ConsumeInternalAsync();
        }

        private int _retryTime = 0;
        private async Task ConsumeInternalAsync()
        {
            try
            {
                if (_channel == null) await CreateChannelAsync();
                AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (ch, ea) =>
                {
                    try
                    {
                        byte[] body = ea.Body.ToArray();
                        MimeMessage message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body)) as MimeMessage;
                        _handler.Send(message);
                        _channel.BasicAckAsync(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                    }
                };
            }
            catch (Exception ex)
            {
                if (_retryTime == 0)
                {
                    await CreateChannelAsync();
                    ConsumeInternalAsync();
                }
            }
            finally
            {
                _retryTime = 0;
            }
        }
    }
}
