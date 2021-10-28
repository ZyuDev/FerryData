using BBComponents.Enums;
using BBComponents.Services;
using FerryData.Engine.Abstract;
using FerryData.Engine.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace FerryData.Client.Pages
{
    public partial class WorkflowSettingsItemEditPage : ComponentBase
    {
        private IWorkflowStepSettings _selectedStep;
        private bool _isWaiting;

        [Parameter]
        public Guid Uid { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public IAlertService AlertService { get; set; }

        public WorkflowSettings Item { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            if (Uid == Guid.Empty)
            {
                // Add new page
                Item = new WorkflowSettings()
                {
                    Title = "New item",
                    Uid = Guid.Empty
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
                        var settingsAsString = await response.Content.ReadAsStringAsync();
                        var workflowSettings = JsonConvert.DeserializeObject<WorkflowSettings>(settingsAsString,
                            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                        Item = workflowSettings;

                        // using System.Text.Json;
                        //Item = await response.Content.ReadFromJsonAsync<WorkflowSettings>(options);
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

            if (Item.Uid == Guid.Empty)
            {
                // Create
                Item.Uid = Guid.NewGuid();
                try
                {
                    var json = JsonConvert.SerializeObject(Item, Formatting.Indented,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    var stringContent = new StringContent(json);

                    _isWaiting = true;
                    var response = await Http.PutAsync("WorkflowSettings/AddItem/", stringContent);
                    _isWaiting = false;


                    if (response.IsSuccessStatusCode)
                    {
                        AlertService.Add("Saved", BootstrapColors.Success);
                    }
                }
                catch (Exception e)
                {
                    var message = $"Cannot add item. Message: {e.Message}";
                    AlertService.Add(message, BootstrapColors.Danger);
                }
            }
            else
            {
                // Update
                try
                {
                    var json = JsonConvert.SerializeObject(Item, Formatting.Indented,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    var stringContent = new StringContent(json);

                    _isWaiting = true;
                    var response = await Http.PostAsync("WorkflowSettings/UpdateItem/", stringContent);
                    _isWaiting = false;

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
