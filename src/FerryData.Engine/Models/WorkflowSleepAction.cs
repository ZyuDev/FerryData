using FerryData.Engine.Abstract;
using FerryData.Engine.Enums;
using System;

namespace FerryData.Engine.Models
{
    [BsonCollection("WorkflowSleepAction")]
    public class WorkflowSleepAction : BaseEntity, IWorkflowStepAction
    {
        public int DelayMilliseconds { get; set; }
        public WorkflowActionKinds Kind { get; } = WorkflowActionKinds.Sleep;

        public WorkflowSleepAction() 
            : base()
        {

        }
    }
}
