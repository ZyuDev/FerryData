using FerryData.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Abstract
{
    public interface IWorkflowStepAction
    {
        Guid Uid { get; set; }
        WorkflowActionKinds Kind { get; }
    }
}
