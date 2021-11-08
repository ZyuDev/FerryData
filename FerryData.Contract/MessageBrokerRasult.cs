using System.Collections.Generic;

namespace FerryData.Contract
{
    public interface IMessageBrokerRasult
    {
        public string Settings { get; set; }

        public Dictionary<string, object> StepsData { get; set; }
    }
}
