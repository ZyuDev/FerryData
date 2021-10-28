using BBComponents.Enums;
using BBComponents.Services;
using FerryData.Client.Connectors;
using FerryData.Engine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FerryData.Client.Pages
{
    public partial class WorkflowSettingsPage : ComponentBase
    {

        private List<WorkflowSettings> _collection = new List<WorkflowSettings>();
        private bool _isWaiting;

        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public IAlertService AlertService { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var connector = new WorkflowSettingsConnector(Http);
            _isWaiting = true;
            _collection = await connector.GetSettingsAsync();
            _isWaiting = false;
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

            var answer = await JsRuntime.InvokeAsync<bool>("confirm", "Delete item?");

            if (!answer)
            {
                return;
            }

            _isWaiting = true;
            try
            {
                var response = await Http.DeleteAsync("WorkflowSettings/RemoveItem/" + item.Uid);

                if (response.IsSuccessStatusCode)
                {
                    _collection.Remove(item);
                    AlertService.Add("Item removed", BootstrapColors.Success);
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
    }
}
