using FerryData.Client.Connectors;
using FerryData.Engine.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FerryData.Client.Pages
{
    public partial class WorkflowRunnerPage: ComponentBase
    {

        private List<WorkflowSettings> _collection = new List<WorkflowSettings>();

        private Guid _selectedSettingsUid;


        [Inject]
        public HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var connector = new WorkflowSettingsConnector(Http);
            _collection = await connector.GetSettingsAsync();
        }

        private void OnRunClicked()
        {

        }
    }
}
