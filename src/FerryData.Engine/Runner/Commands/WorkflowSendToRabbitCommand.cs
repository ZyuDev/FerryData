using FerryData.Contract;
using FerryData.Engine.Abstract;
using FerryData.Engine.Models;
using MassTransit;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Runner.Commands
{
    public class WorkflowSendToRabbitCommand: IWorkflowCommand
    {
        private readonly WorkflowHttpAction _settings;
        private readonly Logger _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private Dictionary<string, object> _stepsData;

        public WorkflowSendToRabbitCommand(WorkflowHttpAction settings,
            Dictionary<string,
                object> stepsData, 
            Logger logger,
            IPublishEndpoint publishEndpoint)
        {
            _settings = settings;
            _stepsData = stepsData;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IWorkflowCommandResult> ExecuteAsync()
        {
            //TODO: Send to rabbit here

            var execResult = new WorkflowStepExecuteResult();

            _logger.Info($"Step send to Rabbit");

            await _publishEndpoint.Publish<IMessageBrokerRasult>(new
            {
                Settings = JsonConvert.SerializeObject(_settings),
                StepsData = _stepsData
            });

            return execResult;
        }
    }
}
