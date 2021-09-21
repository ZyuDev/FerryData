using System;
using System.Collections.Generic;

namespace FerryData.Engine.Abstract
{
    public interface IWorkflow
    {
        Guid Uid { get; set; }

        IWorkflowSettings Settings { get; set; }

        bool Finished { get; set; }

        IEnumerable<IWorkflowStep> Steps { get; }
    }
}
