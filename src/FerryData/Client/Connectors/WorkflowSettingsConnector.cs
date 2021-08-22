using FerryData.Engine.Models;
using FerryData.Shared.Helpers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
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
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _tokenProvider;

        public WorkflowSettingsConnector(HttpClient httpClient, IAccessTokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }

        public async Task<List<WorkflowSettings>> GetSettingsAsync()
        {

            var collection = new List<WorkflowSettings>();
       
            try
            {
                await AddToken();

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

        private async Task AddToken()
        {
            var tokenResult = await _tokenProvider.RequestAccessToken();

            if (tokenResult.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Value}");
            }
        }
    }
}
