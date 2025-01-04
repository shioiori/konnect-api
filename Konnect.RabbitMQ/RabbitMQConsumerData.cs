using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konnect.RabbitMQ
{
    internal class RabbitMQConsumerData
    {
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public RabbitMQConsumerData(string exchangeName, string routingKey)
        {
            ExchangeName = exchangeName;
            RoutingKey = routingKey;
        }
    }
}
