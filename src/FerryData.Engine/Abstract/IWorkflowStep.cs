using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Abstract
{
    public interface IWorkflowStep
    {
        Guid Uid { get; set; }
        IWorkflowStepSettings Settings { get; set; }
        object Data { get; set; }
        bool Finished { get; set; }

    }
}
