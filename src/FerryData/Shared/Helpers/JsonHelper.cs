using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FerryData.Shared.Helpers
{
    public class JsonHelper
    {
        public static string Serialize(object data)
        {
            var result = JsonConvert.SerializeObject(data,
                Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return result;
        }
    }
}
