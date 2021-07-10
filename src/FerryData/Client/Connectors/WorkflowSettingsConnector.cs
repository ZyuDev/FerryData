using FerryData.Engine.Models;
using FerryData.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FerryData.Client.Connectors
{
    public class WorkflowSettingsConnector
    {
        private HttpClient _httpClient;

        public WorkflowSettingsConnector(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<WorkflowSettings>> GetSettingsAsync()
        {

            var collection = new List<WorkflowSettings>();

            try
            {

                var response = await _httpClient.GetAsync("WorkflowSettings/GetCollection");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var parser = new WorkflowSettingsParser();

                    collection = parser.ParseCollection(jsonString);

                }

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Cannot get items. Message: {e.Message}");
            }

            return collection;
        }
    }
}
