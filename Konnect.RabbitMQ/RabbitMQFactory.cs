using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konnect.RabbitMQ
{
    public class RabbitMQFactory
    {
        private RabbitMQConfiguration _configuration;
        public RabbitMQFactory(RabbitMQConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConnectionFactory Build()
        {
            return new ConnectionFactory()
            {
                HostName = _configuration.HostName,
                Port = _configuration.Port,
                UserName = _configuration.UserName,
                Password = _configuration.Password
            };
        }
    }
}
