using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.MessageQueue
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message);
    }

    public interface IMessageConsumer
    {
        Task ConsumeAsync();
    }

}
