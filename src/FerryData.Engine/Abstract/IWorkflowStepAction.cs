using FerryData.Engine.Enums;
//using FerryData.Engine.JsonConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Abstract
{
    //[JsonInterfaceConverter(typeof(IWorkflowStepActionConverter))]
    public interface IWorkflowStepAction
    {
        Guid Uid { get; set; }
        WorkflowActionKinds Kind { get; }
    }
}
