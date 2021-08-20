using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FerryData.Engine.Abstract;
using FerryData.Engine.Models;

namespace FerryData.Engine.JsonConverters
{
    public class IWorkflowStepSettingsConverter : JsonConverter<IWorkflowStepSettings>
    {
        public override IWorkflowStepSettings Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(
            Utf8JsonWriter writer,
            IWorkflowStepSettings value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case null:
                    JsonSerializer.Serialize(writer, (IWorkflowStepSettings)null, options);
                    break;
                default:
                    {
                        var type = value.GetType();
                        JsonSerializer.Serialize(writer, value, type, options);
                        break;
                    }
            }
        }

    }
}
