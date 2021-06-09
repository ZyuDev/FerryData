using FerryData.Engine.Abstract;
using FerryData.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Models
{
    public class WorkflowSleepAction : IWorkflowStepAction
    {
        public Guid Uid { get; set; }
        public int DelayMilliseconds { get; set; }
        public WorkflowActionKinds Kind { get; } = WorkflowActionKinds.Sleep;

    }
}
