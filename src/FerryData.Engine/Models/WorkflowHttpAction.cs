using FerryData.Engine.Abstract;
using FerryData.Engine.Enums;
using System;
using System.Collections.Generic;
// using System.Text.Json;

namespace FerryData.Engine.Models
{
    [BsonCollection("HttpActions")]
    public class WorkflowHttpAction : BaseEntity, IWorkflowStepAction
    {
        public string Url { get; set; }
        public HttpMethods Method { get; set; }
        public bool AutoParse { get; set; } = true;
        public WorkflowActionKinds Kind { get; } = WorkflowActionKinds.HttpConnector;
        public string Body { get; set; }
        public bool SendToRabbit { get; set; }

        public List<NameValueDescriptionRow> Headers { get; set; } = new List<NameValueDescriptionRow>();
        public List<NameValueDescriptionRow> Parameters { get; set; } = new List<NameValueDescriptionRow>();

        public WorkflowHttpAction() 
            : base()
        {

        }

        public NameValueDescriptionRow NewHeaderRow()
        {
            var newRow = new NameValueDescriptionRow();
            Headers.Add(newRow);

            return newRow;
        }

        public void RemoveHeaderRow(NameValueDescriptionRow row)
        {
            Headers.Remove(row);
        }

        public NameValueDescriptionRow NewParameterRow()
        {
            var newRow = new NameValueDescriptionRow();
            Parameters.Add(newRow);

            return newRow;
        }

        public void RemoveParameterRow(NameValueDescriptionRow row)
        {
            Parameters.Remove(row);
        }

        public void UpdateParameters(Dictionary<string, string> parameters)
        {

            var descriptionsCache = new Dictionary<string, string>();
            foreach (var row in Parameters)
            {
                if (string.IsNullOrEmpty(row.Description))
                {
                    continue;
                }

                if (descriptionsCache.ContainsKey(row.Name))
                {
                    continue;
                }

                descriptionsCache.Add(row.Name, row.Description);
            }

            Parameters.Clear();

            foreach (var kvp in parameters)
            {
                var newRow = new NameValueDescriptionRow()
                {
                    Name = kvp.Key,
                    Value = kvp.Value
                };

                if (descriptionsCache.TryGetValue(kvp.Key, out var description))
                {
                    newRow.Description = description;
                }

                Parameters.Add(newRow);
            }
        }

    }
}
