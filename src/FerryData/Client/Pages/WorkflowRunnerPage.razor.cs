using BBComponents.Enums;
using BBComponents.Services;
using FerryData.Client.Connectors;
using FerryData.Engine.Models;
using Microsoft.AspNetCore.Components;
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

        private Guid _selectedSettingsUid;


        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public IAlertService AlertService { get; set; }



        protected override async Task OnInitializedAsync()
        {
            var connector = new WorkflowSettingsConnector(Http);
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

        }
    }
}
