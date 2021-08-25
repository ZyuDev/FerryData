﻿using BBComponents.Enums;
using BBComponents.Services;
using FerryData.Engine.Abstract;
//using FerryData.Engine.JsonConverters;
using FerryData.Engine.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

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

        //private JsonSerializerOptions options {get;} = new JsonSerializerOptions
        //{
        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //    Converters = { new IWorkflowStepSettingsConverter(), new IWorkflowStepActionConverter() },
        //};

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
            try
            {
                // var json = JsonConvert.SerializeObject(Item, Formatting.Indented,
                // new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                // var stringContent = new StringContent(json);

                // var response = await Http.PutAsync("WorkflowSettings/AddItem/", stringContent);

                // using System.Text.Json;
                //var response = await Http.PutAsJsonAsync("WorkflowSettings/AddItem/", Item, options);
                var headers = new Dictionary<string, string>();
                headers.Add("Authorization", "Bearer AQAAAABRxDaVAAce3ScWQiEhzk0joTM5UFpdysM");

                var jsText = "{\"method\": \"get\",\"params\": {\"SelectionCriteria\": {},\"FieldNames\": [\"Id\", \"Name\"]}}";
                object js = JsonConvert.DeserializeObject<ExpandoObject>(jsText, new ExpandoObjectConverter()); ;
 
                var workSet = new WorkflowSettings();
                workSet.Title = "Ya";
                var step = new WorkflowActionStepSettings();
                step.Title = "Ya";
                step.Action = new WorkflowHttpAction {
                    Url = "https://api-sandbox.direct.yandex.com/json/v5/campaigns",
                    Method = Engine.Enums.HttpMethods.Post,
                    Headers = headers,
                    JsonRequest = js,
                };

                workSet.Steps.Add(step);

                // var response = await Http.PutAsJsonAsync("WorkflowSettings/AddItem/", workSet);
                
                var json = JsonConvert.SerializeObject(workSet, Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                var stringContent = new StringContent(json);

                var response = await Http.PutAsync("WorkflowSettings/AddItem/", stringContent);

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
