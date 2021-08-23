using FerryData.Engine.Abstract;
using FerryData.Engine.JsonConverters;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Models
{
    [BsonDiscriminator("WorkflowActionStepSettings")]
    public class WorkflowActionStepSettings : WorkflowStepSettingsBase
    {

        public IWorkflowStepAction Action { get; set; }

        public WorkflowActionStepSettings()
        {
            Kind = Enums.WorkflowStepKinds.Action;
        }
    }
}
