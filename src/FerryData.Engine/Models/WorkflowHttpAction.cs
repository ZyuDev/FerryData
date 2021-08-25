using FerryData.Engine.Abstract;
using FerryData.Engine.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace FerryData.Engine.Models
{
    //[BsonDiscriminator("WorkflowHttpAction")]
    public class WorkflowHttpAction : IWorkflowStepAction
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public string Url { get; set; }
        public HttpMethods Method { get; set; }
        public bool AutoParse { get; set; } = true;
        public WorkflowActionKinds Kind { get; } = WorkflowActionKinds.HttpConnector;
        public Dictionary<string,string> Headers { get; set; }
        public object JsonRequest { get; set; }
    }
}
