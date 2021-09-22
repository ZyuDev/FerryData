using FerryData.Engine.Abstract;
//using FerryData.Engine.JsonConverters;

namespace FerryData.Engine.Models
{
    [BsonCollection("ActionStepSettings")]
    public class WorkflowActionStepSettings : WorkflowStepSettingsBase
    {
        public IWorkflowStepAction Action { get; set; }

        public WorkflowActionStepSettings()
        {
            Kind = Enums.WorkflowStepKinds.Action;
        }
    }
}
