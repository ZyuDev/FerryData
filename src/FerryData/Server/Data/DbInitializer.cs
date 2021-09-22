using FerryData.Engine.Abstract.Service;
using FerryData.Engine.Models;

namespace FerryData.Server.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IMongoService<WorkflowSettings> _settingsService;
        private readonly IMongoService<WorkflowActionStepSettings> _actionStepService;
        private readonly IMongoService<WorkflowSleepAction> _sleepActionService;
        private readonly IMongoService<WorkflowHttpAction> _httpActionService;

        public DbInitializer(IMongoService<WorkflowSettings> settingsService,
            IMongoService<WorkflowActionStepSettings> actionStepService,
            IMongoService<WorkflowSleepAction> sleepActionService,
            IMongoService<WorkflowHttpAction> httpActionService)
        {
            _settingsService = settingsService;
            _actionStepService = actionStepService;
            _sleepActionService = sleepActionService;
            _httpActionService = httpActionService;
        }

        public void InitializeDb()
        {
            if (_settingsService.Count().Result > 0)
            {
                return;
            }

            var workflow1 = new WorkflowSettings() { Title = "Workflow with sleep", Memo = "Workflow with sleep step", Owner = "admin" };

            var sleepAction = new WorkflowSleepAction() { DelayMilliseconds = 1000 };
            _sleepActionService.AddAsync(sleepAction);

            var sleepStep = new WorkflowActionStepSettings()
            {
                Title = "Sleep 1 second",
                Name = "sleep",
                Action = sleepAction
            };

            _actionStepService.AddAsync(sleepStep);

            workflow1.Steps.Add(sleepStep);

            _settingsService.AddAsync(workflow1);


            var workflow2 = new WorkflowSettings() { Title = "Workflow with HTTP", Memo = "Workflow with HTTP GET request", Owner = "admin" };

            var httpAction = new WorkflowHttpAction()
            {
                Url = "http://api.exchangeratesapi.io/v1/latest?access_key=a5cf9da55cb835d0a633a7825b3aa8b5"
            };

            _httpActionService.AddAsync(httpAction);

            var httpStep = new WorkflowActionStepSettings()
            {
                Title = "Get rates",
                Name = "get_rates",
                Action = httpAction
            };

            _actionStepService.AddAsync(httpStep);

            workflow2.AddStep(httpStep);

            _settingsService.AddAsync(workflow2);

            _settingsService.AddAsync(new WorkflowSettings() { Title = "Workflow without steps", Memo = "Empty workflow", Owner = "admin" });
        }
    }
}
