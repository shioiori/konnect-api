using Microsoft.Extensions.Configuration;
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

        internal RabbitMQFactory() { }

        public IConnectionFactory Build()
        {
            if (_configuration == null) Init();
            return new ConnectionFactory()
            {
                HostName = _configuration.HostName,
                Port = _configuration.Port,
                UserName = _configuration.UserName,
                Password = _configuration.Password
            };
        }

        internal void Init()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
            _configuration = config.GetSection("RabbitMQ").Get<RabbitMQConfiguration>();
        }
    }
}
