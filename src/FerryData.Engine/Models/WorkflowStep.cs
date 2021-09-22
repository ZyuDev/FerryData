using FerryData.Engine.Abstract;
using System;

namespace FerryData.Engine.Models
{
    [BsonCollection("Steps")]
    public class WorkflowStep : BaseEntity, IWorkflowStep
    {
        public bool Finished { get; set; }
        public object Data { get; set; }
        public IWorkflowStepSettings Settings { get; set; }

        public WorkflowStep() 
            : base()
        {

        }
    }
}
