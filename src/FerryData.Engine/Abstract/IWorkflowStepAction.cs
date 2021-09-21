using FerryData.Engine.Enums;
//using FerryData.Engine.JsonConverters;
using System;

namespace FerryData.Engine.Abstract
{
    //[JsonInterfaceConverter(typeof(IWorkflowStepActionConverter))]
    public interface IWorkflowStepAction
    {
        Guid Uid { get; set; }
        WorkflowActionKinds Kind { get; }
    }
}
