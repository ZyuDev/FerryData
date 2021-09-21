using FerryData.Engine.Enums;
using FerryData.Engine.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace FerryData.Shared.Helpers
{
    public class WorkflowSettingsParser
    {
        public WorkflowSettings Parse(string jsonString)
        {
            var jObject = JsonConvert.DeserializeObject<JObject>(jsonString);
            var settings = Parse(jObject);

            return settings;
        }

        public WorkflowSettings Parse(JObject jObject)
        {
            var settings = new WorkflowSettings();

            var settingsUidStr = GetObjectPropertyValue<string>(jObject, "uid");
            settings.Uid = Guid.Parse(settingsUidStr);
            settings.Title = GetObjectPropertyValue<string>(jObject, "title");
            settings.Memo = GetObjectPropertyValue<string>(jObject, "memo");

            var jArray = (JArray)jObject["steps"];
            foreach (JObject jStep in jArray)
            {
                var kind = (WorkflowStepKinds)GetObjectPropertyValue<int>(jStep, "kind");

                if (kind == WorkflowStepKinds.Action)
                {
                    var step = new WorkflowActionStepSettings();

                    var stepUidStr = GetObjectPropertyValue<string>(jStep, "uid");
                    step.Uid = Guid.Parse(stepUidStr);

                    var inStepUidStr = GetObjectPropertyValue<string>(jStep, "inUid");
                    step.InUid = Guid.Parse(inStepUidStr);

                    step.Title = GetObjectPropertyValue<string>(jStep, "title");
                    step.Name = GetObjectPropertyValue<string>(jStep, "name");
                    step.Memo = GetObjectPropertyValue<string>(jStep, "memo");

                    var jAction = jStep["action"];
                    if (jAction != null && jAction.HasValues)
                    {
                        var jActionObject = (JObject)jAction;

                        var actionKind = (WorkflowActionKinds)GetObjectPropertyValue<int>(jActionObject, "kind");

                        if (actionKind == WorkflowActionKinds.Sleep)
                        {
                            step.Action = jActionObject.ToObject<WorkflowSleepAction>();
                        }
                        else if (actionKind == WorkflowActionKinds.HttpConnector)
                        {
                            step.Action = jActionObject.ToObject<WorkflowHttpAction>();
                        }
                    }

                    settings.AddStep(step);
                }
            }

            return settings;
        }

        public List<WorkflowSettings> ParseCollection(string jsonString)
        {
            var collection = new List<WorkflowSettings>();

            var jArray = JsonConvert.DeserializeObject<JArray>(jsonString);

            foreach (JObject jObject in jArray)
            {
                var settingsItem = Parse(jObject);
                collection.Add(settingsItem);
            }

            return collection;
        }

        public T GetObjectPropertyValue<T>(JObject jObject, string key)
        {
            var value = default(T);
            var jToken = jObject[key];
            if (jToken is JValue jValue)
            {
                value = jValue.Value<T>();

            }

            return value;

        }
    }
}
