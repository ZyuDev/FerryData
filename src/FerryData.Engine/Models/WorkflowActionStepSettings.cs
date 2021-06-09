using FerryData.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Models
{
    public class WorkflowActionStepSettings : WorkflowStepSettingsBase
    {

        public IWorkflowStepAction Action { get; set; }

        public WorkflowActionStepSettings()
        {
            Kind = Enums.WorkflowStepKinds.Action;
        }
    }
}
