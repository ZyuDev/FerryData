using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FerryData.Engine.Abstract;
using FerryData.Engine.Enums;
using FerryData.Engine.Models;

namespace FerryData.Engine.JsonConverters
{
    public class IWorkflowStepActionConverter : JsonConverter<IWorkflowStepAction>
    {
        enum TypeDiscriminator
        {
            WorkflowHttpAction = 1,
            WorkflowSleepAction = 2,
        }

        public override bool CanConvert(Type typeToConvert) =>
            typeof(IWorkflowStepAction).IsAssignableFrom(typeToConvert);

        public override IWorkflowStepAction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = reader.GetString();
            if (propertyName != "TypeDiscriminator")
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            TypeDiscriminator typeDiscriminator = (TypeDiscriminator)reader.GetInt32();
            IWorkflowStepAction _IWorkflowStepAction = typeDiscriminator switch
            {
                TypeDiscriminator.WorkflowHttpAction => new WorkflowHttpAction(),
                TypeDiscriminator.WorkflowSleepAction => new WorkflowSleepAction(),
                _ => throw new JsonException()
            };

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return _IWorkflowStepAction;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        // IWorkflowStepAction - интерфейс
                        case "Uid":
                            var uid = reader.GetGuid();
                            _IWorkflowStepAction.Uid = uid;
                            break;

                        // WorkflowSleepAction - класс
                        case "DelayMilliseconds":
                            var delay = reader.GetInt32();
                            ((WorkflowSleepAction)_IWorkflowStepAction).DelayMilliseconds = delay;
                            break;

                        // WorkflowHttpAction - класс
                        case "Url":
                            var url = reader.GetString();
                            ((WorkflowHttpAction)_IWorkflowStepAction).Url = url;
                            break;
                        case "Method":
                            var method = (HttpMethods)reader.GetInt32();
                            ((WorkflowHttpAction)_IWorkflowStepAction).Method = method;
                            break;
                        case "AutoParse":
                            var autoparse = reader.GetBoolean();
                            ((WorkflowHttpAction)_IWorkflowStepAction).AutoParse = autoparse;
                            break;
                        case "Headers":
                            var headers = JsonSerializer.Deserialize<Dictionary<string,string>>(reader.GetString());
                            ((WorkflowHttpAction)_IWorkflowStepAction).Headers = headers;
                            break;
                        case "JsonRequest":
                            var jsonRequest = reader.GetString();
                            ((WorkflowHttpAction)_IWorkflowStepAction).JsonRequest = jsonRequest;
                            break;
                            //default:
                            //   break;

                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, IWorkflowStepAction value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value is WorkflowSleepAction sleepAction)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.WorkflowSleepAction);
                writer.WriteNumber("DelayMilliseconds", sleepAction.DelayMilliseconds);
            }
            else if (value is WorkflowHttpAction httpAction)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.WorkflowHttpAction);
                writer.WriteString("Url", httpAction.Url);
                writer.WriteNumber("Method", (int)httpAction.Method);
                writer.WriteBoolean("AutoParse", httpAction.AutoParse);
                writer.WriteString("Headers", JsonSerializer.Serialize(httpAction.Headers));
                if (httpAction.JsonRequest != null)
                {
                    //var t = ToJsonString(httpAction.JsonRequest);
                    writer.WriteString("JsonRequest", httpAction.JsonRequest);
                }
            }

            writer.WriteString("Uid", value.Uid);

            writer.WriteEndObject();
        }

        public static string ToJsonString(JsonDocument jdoc)
        {
            using (var stream = new MemoryStream())
            {
                Utf8JsonWriter writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
                jdoc.WriteTo(writer);
                writer.Flush();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
