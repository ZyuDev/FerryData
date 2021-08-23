using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FerryData.Engine.Abstract;
using FerryData.Engine.Enums;
using FerryData.Engine.Models;

namespace FerryData.Engine.JsonConverters
{
    public class IWorkflowStepSettingsConverter : JsonConverter<IWorkflowStepSettings>
    {
        enum TypeDiscriminator
        {
            WorkflowActionStepSettings = 1
        }

        public override bool CanConvert(Type typeToConvert) =>
            typeof(IWorkflowStepSettings).IsAssignableFrom(typeToConvert);

        public override IWorkflowStepSettings Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
            IWorkflowStepSettings _IWorkflowStepSettings = typeDiscriminator switch
            {
                TypeDiscriminator.WorkflowActionStepSettings => new WorkflowActionStepSettings(),
                _ => throw new JsonException()
            };

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return _IWorkflowStepSettings;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "Uid":
                            var uid = reader.GetGuid();
                            ((WorkflowActionStepSettings)_IWorkflowStepSettings).Uid = uid;
                            break;
                        case "Title":
                            var title = reader.GetString();
                            ((WorkflowActionStepSettings)_IWorkflowStepSettings).Title = title;
                            break;
                        case "Name":
                            var name = reader.GetString();
                            ((WorkflowActionStepSettings)_IWorkflowStepSettings).Name = name;
                            break;
                        case "Memo":
                            var memo = reader.GetString();
                            ((WorkflowActionStepSettings)_IWorkflowStepSettings).Memo = memo;
                            break;
                        case "Kind":
                            var kind = (WorkflowStepKinds)reader.GetInt32();
                            ((WorkflowActionStepSettings)_IWorkflowStepSettings).Kind = kind;
                            break;
                        case "Action":
                            var action = JsonSerializer.Deserialize<IWorkflowStepAction>(reader.GetString());
                            ((WorkflowActionStepSettings)_IWorkflowStepSettings).Action = action;
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(
            Utf8JsonWriter writer, IWorkflowStepSettings _IWorkflowStepSettings, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (_IWorkflowStepSettings is WorkflowActionStepSettings _WorkflowActionStepSettings)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.WorkflowActionStepSettings);
                writer.WriteString("Uid", _WorkflowActionStepSettings.Uid.ToString());
                writer.WriteString("Title", _WorkflowActionStepSettings.Title);
                writer.WriteString("Name", _WorkflowActionStepSettings.Name);
                writer.WriteString("Memo", _WorkflowActionStepSettings.Memo);
                writer.WriteNumber("Kind", (int)_WorkflowActionStepSettings.Kind);

                // TODO при сериализации объекта появляются UTF-8 артифакты,
                // а именно \u2022 - символ "ковычки" в UTF-8. 
                if (_WorkflowActionStepSettings.Action != null)
                    writer.WriteString("Action", JsonSerializer.Serialize(_WorkflowActionStepSettings.Action,options));
            }

            writer.WriteEndObject();
        }
    }
}
