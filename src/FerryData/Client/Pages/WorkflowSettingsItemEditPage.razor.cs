using FerryData.Engine.Abstract;
using FerryData.Engine.Models;
using FerryData.Shared.Helpers;
using FerryData.Shared.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Client.Pages
{
    public partial class WorkflowSettingsItemEditPage : ComponentBase
    {
        private IWorkflowStepSettings _selectedStep;

        [Parameter]
        public Guid Uid { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        public HttpClient Http { get; set; }

        public WorkflowSettings Item { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (Uid == Guid.Empty)
            {
                // Add new page
                Item = new WorkflowSettings()
                {
                    Title = "New item"
                };
            }
            else
            {
                // Load item

                try
                {

                    var response = await Http.GetAsync($"WorkflowSettings/GetItem/{Uid}");
                    if (response.IsSuccessStatusCode)
                    {
                        var parser = new WorkflowSettingsParser();
                        var json = await response.Content.ReadAsStringAsync();
                        Item = parser.Parse(json);

                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Cannot get item. Message: {e.Message}");
                }

            }
        }

        private void OnReturnClick()
        {
            NavManager.NavigateTo("/WorkflowSettings");
        }

        private async Task OnSaveClick()
        {


            try
            {

                // Serialize by Newtonsoft because standard serializer do not serialize actions.
                var json = JsonConvert.SerializeObject(Item,
                    Formatting.Indented,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await Http.PostAsync("WorkflowSettings/UpdateItem", jsonContent);

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Cannot update item. Message: {e.Message}");
            }
        }

        private void OnAddSleepStepClick()
        {
            if (Item == null)
            {
                return;
            }

            var newStep = new WorkflowActionStepSettings
            {
                Title = "New sleep step",
                Name = "new_sleep_step",
                Action = new WorkflowSleepAction()
                {
                    DelayMilliseconds = 1000
                }
            };

            Item.AddStep(newStep);

            _selectedStep = newStep;

        }

        private void OnAddHttpStepClick()
        {
            if (Item == null)
            {
                return;
            }

            var newStep = new WorkflowActionStepSettings
            {
                Title = "New HTTP step",
                Name = "new_http_step",
                Action = new WorkflowHttpAction()
                {
                    AutoParse = true,
                    Method = Engine.Enums.HttpMethods.Get
                }
            };

            Item.AddStep(newStep);

            _selectedStep = newStep;
        }

        private void OnRemoveStepClick(IWorkflowStepSettings step)
        {
            if (Item == null)
            {
                return;
            }

            Item.RemoveStep(step);
        }

        private void OnStepClick(IWorkflowStepSettings step)
        {
            _selectedStep = step;
        }
    }
}
