using BBComponents.Enums;
using BBComponents.Services;
using FerryData.Engine.Abstract;
using FerryData.Engine.JsonConverters;
using FerryData.Engine.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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

        [Inject]
        public IAlertService AlertService { get; set; }

        public WorkflowSettings Item { get; set; }

        private JsonSerializerOptions options {get;} = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new IWorkflowStepSettingsConverter(), new IWorkflowStepActionConverter() },
        };

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
                        Item = await response.Content.ReadFromJsonAsync<WorkflowSettings>(options);
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
                var response = await Http.PutAsJsonAsync("WorkflowSettings/AddItem/", Item, options);
                
                if (response.IsSuccessStatusCode)
                {
                    AlertService.Add("Saved", BootstrapColors.Success);
                }
            }
            catch (Exception e)
            {
                var message = $"Cannot update item. Message: {e.Message}";
                AlertService.Add(message, BootstrapColors.Danger);
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
