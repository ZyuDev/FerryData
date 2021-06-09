using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
