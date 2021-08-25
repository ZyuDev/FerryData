using FerryData.Engine.Abstract;
using FerryData.Engine.Enums;
using FerryData.Engine.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace FerryData.Engine.Runner
{
    public class WorkflowRunner
    {
        private readonly IWorkflowSettings _settings;
        private Dictionary<string, object> _stepsData;

        private Logger _logger;
        private IList<string> _messages = new List<string>();

        public Workflow Workflow { get; private set; }

        public IEnumerable<string> Messages => _messages;

        public WorkflowRunner(IWorkflowSettings settings)
        {
            _settings = settings;

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

        public async Task<WorkflowStepExecuteResult> ExecuteStepAsync(IWorkflowStep step)
        {
            var execResult = new WorkflowStepExecuteResult();

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
                    _logger.Info($"Start executing step {stepSettings.Action.Kind}:{stepSettings}");

                    if (stepSettings.Action.Kind == WorkflowActionKinds.Sleep)
                    {
                        var timerAction = (WorkflowSleepAction)stepSettings.Action;

                        _logger.Info($"Sleep started for {timerAction.DelayMilliseconds}");
                        await Task.Delay(timerAction.DelayMilliseconds);

                        _logger.Info($"Resume after sleep");
                    }
                    else if (stepSettings.Action.Kind == WorkflowActionKinds.HttpConnector)
                    {


                        execResult = await ExecuteHttpStep(stepSettings, _stepsData);
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
               // _stepsData.Add(step.Settings.Name, step.Data);
            }

            return execResult;
        }

        public async Task<WorkflowStepExecuteResult> ExecuteHttpStep(WorkflowActionStepSettings stepSettings, Dictionary<string, object> stepsData)
        {

            var execResult = new WorkflowStepExecuteResult();


            var httpActionSettings = (WorkflowHttpAction)stepSettings.Action;

            if (httpActionSettings.Method == HttpMethods.Get)
            {
                var httpClient = new HttpClient();

                var url = httpActionSettings.Url;

                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.Info($"Request done");

                    if (httpActionSettings.AutoParse)
                    {
                        var resultObject = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());
                        execResult.Data = resultObject;
                    }
                    else
                    {
                        execResult.Data = content;
                    }

                }
                else
                {
                    execResult.Status = -1;
                    var message = $"Request error: {response.StatusCode} {response.ReasonPhrase} {response.RequestMessage}";
                    _logger.Error(message);
                    execResult.Message = message;
                }

            }
            else if(httpActionSettings.Method == HttpMethods.Post)
            {
                var httpClient = new HttpClient();

                var url = httpActionSettings.Url;

                var requestJson = JsonConvert.SerializeObject(httpActionSettings.JsonRequest);

                var stringContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(httpActionSettings.Headers.Keys.ToArray()[0], httpActionSettings.Headers.Values.ToArray()[0]);

                var response = await httpClient.PostAsync(url, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.Info($"Request done");

                    if (httpActionSettings.AutoParse)
                    {
                        var resultObject = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());
                        execResult.Data = resultObject;
                    }
                    else
                    {
                        execResult.Data = content;
                    }

                }
                else
                {
                    execResult.Status = -1;
                    var message = $"Request error: {response.StatusCode} {response.ReasonPhrase} {response.RequestMessage}";
                    _logger.Error(message);
                    execResult.Message = message;
                }
            }

            return execResult;

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
