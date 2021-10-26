using FerryData.Engine.Enums;
using FerryData.Engine.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Runner
{
    public class WorkflowHttpConnector
    {
        private readonly WorkflowHttpAction _httpActionSettings;
        private readonly Dictionary<string, object> _stepsData;
        private readonly Logger _logger;

        public WorkflowHttpConnector(WorkflowHttpAction httpActionSettings, Dictionary<string, object> stepsData, Logger logger)
        {
            _httpActionSettings = httpActionSettings;
            _stepsData = stepsData;
            _logger = logger;

        }

        public async Task<WorkflowStepExecuteResult> Execute()
        {
            var execResult = new WorkflowStepExecuteResult();

            // Prepare Url.
            var url = _httpActionSettings.Url;
            if (TemplateParser.TemplateContainsExpressions(url))
            {
                url = TemplateParser.PrepareTemplate(url, _stepsData, _logger);
            }

            // Prepare body.
            string body = "";
            if (!string.IsNullOrEmpty(_httpActionSettings.Body))
            {
                body = _httpActionSettings.Body;
                if (TemplateParser.TemplateContainsExpressions(body))
                {
                    body = TemplateParser.PrepareTemplate(body, _stepsData, _logger);
                }
            }

            using (var httpClient = new HttpClient())
            {


                // Set headers
                if (_httpActionSettings.Headers.Any())
                {
                    foreach (var headerRow in _httpActionSettings.Headers)
                    {

                        var headerValue = headerRow.Value;

                        if (TemplateParser.TemplateContainsExpressions(headerValue))
                        {
                            headerValue = TemplateParser.PrepareTemplate(headerValue, _stepsData, _logger);
                        }

                        if (headerRow.Name.ToLower().Contains("content-type"))
                        {
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(headerValue));
                        }
                        else
                        {
                            httpClient.DefaultRequestHeaders.Add(headerRow.Name, headerValue);
                        }

                    }
                }

                HttpResponseMessage response = null;
                if (_httpActionSettings.Method == HttpMethods.Get)
                {

                    var request = new HttpRequestMessage(HttpMethod.Get, url);

                    if (!string.IsNullOrEmpty(body))
                    {
                        StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                        request.Content = content;
                    }

                    response = await httpClient.SendAsync(request);


                }
                else
                {
                    // POST
                    StringContent content = new StringContent(body, Encoding.UTF8, "application/json");

                    response = await httpClient.PostAsync(url, content);

                }

                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        _logger.Info($"Request done");

                        if (_httpActionSettings.AutoParse)
                        {
                            execResult.Data = AutoParse(content);
                        }
                        else
                        {
                            execResult.Data = content;
                        }

                    }
                    else
                    {
                        execResult.Status = -1;
                        var message = $"Request error: {response.StatusCode} {response.ReasonPhrase} {response.RequestMessage}";
                        _logger.Error(message);
                        execResult.Message = message;
                    }
                }


            }
            return execResult;

        }

        private object AutoParse(string content)
        {

            object resultObject = null;
            if (!string.IsNullOrEmpty(content))
            {

                if (content.Trim().Substring(0, 1) == "[")
                {
                    resultObject = JsonConvert.DeserializeObject<List<ExpandoObject>>(content, new ExpandoObjectConverter());
                }
                else
                {
                    resultObject = JsonConvert.DeserializeObject<ExpandoObject>(content, new ExpandoObjectConverter());
                }
            }

            return resultObject;
        }
    }
}
