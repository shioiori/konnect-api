using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.MessageQueue
{
    public interface IMessagePublisher
    {
        void Publish<T>(T message);
    }

    public interface IMessageConsumer
    {
        void Consume<T>(string queue, Action<T> messageHandler);
    }

    public interface IMessageQueueConfiguration
    {

    }
}
