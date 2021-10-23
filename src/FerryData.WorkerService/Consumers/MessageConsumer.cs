using FerryData.Contract;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.WorkerService.Consumers
{
    public class MessageConsumer : IConsumer<IMessageBrokerRasult>
    {
        public Task Consume(ConsumeContext<IMessageBrokerRasult> context)
        {
            throw new NotImplementedException();
        }
    }
}
