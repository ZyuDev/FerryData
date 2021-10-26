using FerryData.Engine.Abstract;
using FerryData.Engine.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Runner.Commands
{
    public class WorkflowHttpCommand : IWorkflowCommand
    {
        private readonly WorkflowHttpAction _settings;
        private readonly Logger _logger;
        private Dictionary<string, object> _stepsData;

        public WorkflowHttpCommand(WorkflowHttpAction settings, Dictionary<string, object> stepsData, Logger logger)
        {
            _settings = settings;
            _stepsData = stepsData;
            _logger = logger;
        }

        public async Task<IWorkflowCommandResult> ExecuteAsync()
        {
            var connector = new WorkflowHttpConnector(_settings, _stepsData, _logger);

            var execResult = await connector.Execute();

            return execResult;

        }
    }
}
