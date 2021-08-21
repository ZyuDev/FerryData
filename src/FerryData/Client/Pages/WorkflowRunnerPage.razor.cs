using BBComponents.Enums;
using BBComponents.Services;
using FerryData.Client.Connectors;
using FerryData.Engine.Models;
using FerryData.Engine.Runner;
using FerryData.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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


        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public IAlertService AlertService { get; set; }

        [Inject]
        public IAccessTokenProvider TokenProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var connector = new WorkflowSettingsConnector(Http, TokenProvider);
            _collection = await connector.GetSettingsAsync();
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
            try
            {
                var payload = new { uid = selectedSettings.Uid };
                var json = JsonConvert.SerializeObject(payload,
                            Formatting.Indented,
                            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await Http.PostAsync($"WorkflowRunner/Execute/", jsonContent);

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
                AlertService.Add($"Cannot remove item. Message: {e.Message}", BootstrapColors.Danger);
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
    }
}
