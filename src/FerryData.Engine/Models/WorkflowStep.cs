using FerryData.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Models
{
    public class WorkflowStep : IWorkflowStep
    {
        public Guid Uid { get; set; }
        public bool Finished { get; set; }
        public object Data { get; set; }
        public IWorkflowStepSettings Settings { get; set; }
    }
}
