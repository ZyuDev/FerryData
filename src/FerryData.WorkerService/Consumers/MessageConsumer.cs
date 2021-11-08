using FerryData.Contract;
using FerryData.Engine.Models;
using FerryData.Engine.Runner.Commands;
using MassTransit;
using Newtonsoft.Json;
using NLog;
using System.Threading.Tasks;

namespace FerryData.WorkerService.Consumers
{
    public class MessageConsumer : IConsumer<IMessageBrokerRasult>
    {
        public async Task Consume(ConsumeContext<IMessageBrokerRasult> context)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var settings = JsonConvert.DeserializeObject<WorkflowHttpAction>(context.Message.Settings);

            var stepData = context.Message.StepsData;

            var command = new WorkflowHttpCommand(settings, stepData, logger);

            var result =  await command.ExecuteAsync();

            ;
        }
    }
}
