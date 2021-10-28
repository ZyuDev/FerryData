using FerryData.Engine.Abstract;
using FerryData.Engine.Abstract.Service;
using FerryData.Engine.Enums;
using FerryData.Engine.Models;
using FerryData.Engine.Runner.Commands;
using Newtonsoft.Json;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FerryData.Engine.Runner
{
    public class WorkflowRunner
    {
        private readonly IWorkflowSettings _settings;
        private readonly IRunnerMemoryCacheService _cacheService;

        private Dictionary<string, object> _stepsData;

        private Logger _logger;
        private IList<string> _messages = new List<string>();

        public Workflow Workflow { get; private set; }

        public IEnumerable<string> Messages => _messages;

        public WorkflowRunner(IWorkflowSettings settings, IRunnerMemoryCacheService cacheService)
        {
            _settings = settings;
            _cacheService = cacheService;

            _stepsData = new Dictionary<string, object>();

            InitLogger();
            InitWorkflow(settings);


        }

        public async Task Run()
        {

            if (!Workflow.Steps.Any())
            {
                _logger.Error("Empty workflow");
            }

            var flagContinue = true;
            var currentStep = Workflow.GetStartStep();

            flagContinue = currentStep != null;

            while (flagContinue)
            {
                var execResult = await ExecuteStepAsync(currentStep);

                if (_cacheService != null)
                {
                    var currentResult = PrepareExecuteResult();
                    _cacheService.SetResult(currentResult);
                }

                currentStep = Workflow.GetNextStep(currentStep);
                if (currentStep == null)
                {
                    currentStep = Workflow.GetStartStep();
                }

                flagContinue = currentStep != null;

            }

            var target = LogManager.Configuration.FindTargetByName<MemoryTarget>("in_memory_log");
            _messages = target.Logs;
        }

        public async Task<IWorkflowCommandResult> ExecuteStepAsync(IWorkflowStep step)
        {
            IWorkflowCommandResult execResult = new WorkflowStepExecuteResult();

            if (step.Settings.Kind == WorkflowStepKinds.Action)
            {
                var stepSettings = (WorkflowActionStepSettings)step.Settings;

                if (stepSettings.Action == null)
                {
                    var message = $"Empty action in step {step.Settings.Title}.";
                    execResult.Message = message;
                    _logger.Error(message);

                }
                else
                {

                    var command = WorkflowCommandFactory.Create(stepSettings.Action, _stepsData, _logger);

                    if (command == null)
                    {
                        _logger.Info($"Cannot define command.");
                    }
                    else
                    {
                        _logger.Info($"Start executing step {stepSettings.Action.Kind}:{stepSettings}");
                        execResult = await command.ExecuteAsync();
                        step.Data = execResult.Data;
                    }

                }

            }
            else
            {
                var message = $"Unknown action kind {step.Settings.Kind} in step {step.Settings.Title}.";
                execResult.Status = -1;
                execResult.Message = message;
                _logger.Error(execResult.Message);
            }

            step.Finished = true;
            if (step.Data != null)
            {
                _stepsData.Add(step.Settings.Name, step.Data);
            }

            return execResult;
        }

        public WorkflowExecuteResultDto PrepareExecuteResult()
        {
            var executeResult = new WorkflowExecuteResultDto();

            var target = LogManager.Configuration.FindTargetByName<MemoryTarget>("in_memory_log");

            executeResult.LogMessages.AddRange(target.Logs);

            foreach (var step in this.Workflow.Steps)
            {
                var stepDto = new WorkflowStepExecuteResultDto()
                {
                    Uid = step.Uid,
                    Title = step.Settings.Title,
                    Finished = step.Finished,
                };

                if (step.Data != null)
                {
                    stepDto.Data = JsonConvert.SerializeObject(step.Data);
                }
                executeResult.StepResults.Add(stepDto);
            }

            return executeResult;
        }

        private void InitLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logInMemory = new NLog.Targets.MemoryTarget("in_memory_log");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logInMemory);

            // Apply config           
            LogManager.Configuration = config;

            _logger = LogManager.GetCurrentClassLogger();

        }

        private void InitWorkflow(IWorkflowSettings settings)
        {
            Workflow = new Workflow();
            Workflow.Uid = settings.Uid;

            Workflow.Settings = settings;

            foreach (var settingsStep in Workflow.Settings.Steps)
            {
                var newStep = new WorkflowStep();
                newStep.Uid = settingsStep.Uid;
                newStep.Settings = settingsStep;

                Workflow.AddStep(newStep);
            }
        }

    }
}
