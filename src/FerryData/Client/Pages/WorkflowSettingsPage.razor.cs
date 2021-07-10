using FerryData.Client.Connectors;
using FerryData.Engine.Models;
using FerryData.Shared.Helpers;
using FerryData.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FerryData.Client.Pages
{
    public partial class WorkflowSettingsPage: ComponentBase
    {

        private List<WorkflowSettings> _collection = new List<WorkflowSettings>();

        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        public HttpClient Http { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var connector = new WorkflowSettingsConnector(Http);
            _collection = await connector.GetSettingsAsync();
        }

        private void OnAddNewClicked()
        {
            NavManager.NavigateTo("/WorkflowSettings/Add");

        }

        private void OnItemClick(WorkflowSettings item)
        {
            NavManager.NavigateTo($"/WorkflowSettings/Edit/{item.Uid}");
        }

        private async Task OnItemRemoveClick(WorkflowSettings item)
        {
            try
            {
                var response = await Http.PostAsJsonAsync<WorkflowSettings>("WorkflowSettings/RemoveItem", item);

                if (response.IsSuccessStatusCode)
                {
                    _collection.Remove(item);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Cannot remove item. Message: {e.Message}");
            }
        }
    }
}
