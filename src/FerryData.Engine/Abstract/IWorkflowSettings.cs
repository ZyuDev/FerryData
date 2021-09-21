using System;
using System.Collections.Generic;

namespace FerryData.Engine.Abstract
{
    public interface IWorkflowSettings
    {
        Guid Uid { get; set; }
        string Title { get; set; }
        string Memo { get; set; }

        List<IWorkflowStepSettings> Steps { get; set; }

        void AddStep(IWorkflowStepSettings step);
        void RemoveStep(IWorkflowStepSettings step);
        void ClearSteps();
    }
}
