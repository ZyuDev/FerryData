using BBComponents.Enums;
using BBComponents.Services;
using FerryData.Client.Connectors;
using FerryData.Engine.Models;
using FerryData.Engine.Runner;
using FerryData.Shared.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FerryData.Client.Pages
{
    public partial class WorkflowRunnerPage : ComponentBase
    {

        private List<WorkflowSettings> _collection = new List<WorkflowSettings>();
        private WorkflowExecuteResultDto _execResult = new WorkflowExecuteResultDto();
        private WorkflowStepExecuteResultDto _selectedStepResult;

        private Guid _selectedSettingsUid;
        private bool _isWaiting;
        private bool _isDataModalOpen;

        private Guid _runUid;
        private Timer _timer;


        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public IAlertService AlertService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var connector = new WorkflowSettingsConnector(Http);
            _collection = await connector.GetSettingsAsync();
        }

        private async Task OnStartClicked()
        {
            var selectedSettings = _collection.FirstOrDefault(x => x.Uid == _selectedSettingsUid);

            if (selectedSettings == null)
            {
                AlertService.Add("Settings not selected", BootstrapColors.Warning);
                return;
            }

            _runUid = Guid.NewGuid();
            _isWaiting = true;

            try
            {
                var payload = new WorkflowStartDto() { 
                    SettingsUid = _selectedSettingsUid,
                    RunUid = _runUid
                };
                var json = JsonConvert.SerializeObject(payload, Formatting.Indented,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await Http.PostAsync($"WorkflowRunner/Start/", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    AlertService.Add("Workflow started", BootstrapColors.Success);

                    _execResult = new WorkflowExecuteResultDto();

                    _timer = new Timer();
                    _timer.Interval = 500;
                    _timer.Elapsed += _timer_Elapsed;
                    _timer.Start();
                    

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseDto = JsonConvert.DeserializeObject<ResponseDto<int>>(responseContent);
                }
                else
                {
                    AlertService.Add(response.ReasonPhrase, BootstrapColors.Danger);
                }

            }
            catch (Exception e)
            {
                AlertService.Add($"Cannot start workflow. Message: {e.Message}", BootstrapColors.Danger);
                _isWaiting = false;

            }

        }

        private async void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await CheckWorkflowResult();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OnCheckClicked()
        {
            await CheckWorkflowResult();
        }

        private async Task OnRunClicked()
        {
            var selectedSettings = _collection.FirstOrDefault(x => x.Uid == _selectedSettingsUid);

            if (selectedSettings == null)
            {
                AlertService.Add("Settings not selected", BootstrapColors.Warning);
                return;
            }

            _isWaiting = true;
            _execResult = new WorkflowExecuteResultDto();

            try
            {

                var response = await Http.GetAsync($"WorkflowRunner/Execute/" + selectedSettings.Uid);

                if (response.IsSuccessStatusCode)
                {
                    AlertService.Add("Done", BootstrapColors.Success);

                    var responseContent = await response.Content.ReadAsStringAsync();

                    _execResult = JsonConvert.DeserializeObject<WorkflowExecuteResultDto>(responseContent);
                }
                else
                {
                    AlertService.Add(response.ReasonPhrase, BootstrapColors.Danger);
                }

            }
            catch (Exception e)
            {
                AlertService.Add($"Cannot execute workflow. Message: {e.Message}", BootstrapColors.Danger);
            }
            finally
            {
                _isWaiting = false;
            }

        }

        private void OnStepDataOpen(WorkflowStepExecuteResultDto stepResult)
        {
            _selectedStepResult = stepResult;
            _isDataModalOpen = true;
        }

        private void OnStepDataModalClosed()
        {
            _isDataModalOpen = false;
        }

        private void OnSettingsChanged()
        {
            _execResult = new WorkflowExecuteResultDto();
        }

        private async Task CheckWorkflowResult()
        {
            try
            {
                var response = await Http.GetAsync($"WorkflowRunner/Check/" + _runUid);

                if (response.IsSuccessStatusCode)
                {
                    //AlertService.Add("Check Done", BootstrapColors.Success);

                    var responseContent = await response.Content.ReadAsStringAsync();

                    _execResult = JsonConvert.DeserializeObject<WorkflowExecuteResultDto>(responseContent);

                    if (_execResult.AllStepsDone())
                    {
                        _isWaiting = false;

                        if (_timer != null)
                        {
                            _timer.Stop();
                        }

                        AlertService.Add("Workflow finished", BootstrapColors.Success);

                    }

                }
                else
                {
                    AlertService.Add(response.ReasonPhrase, BootstrapColors.Danger);
                }

            }
            catch (Exception e)
            {
                AlertService.Add($"Cannot check run result. Message: {e.Message}", BootstrapColors.Danger);

            }
        }
    }
}
