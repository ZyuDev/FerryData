using FerryData.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Models
{
    public class WorkflowSettings : IWorkflowSettings
    {
        private List<IWorkflowStepSettings> _steps = new List<IWorkflowStepSettings>();

        public Guid Uid { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Memo { get; set; }

        public IEnumerable<IWorkflowStepSettings> Steps => _steps;

        public void AddStep(IWorkflowStepSettings step)
        {
            _steps.Add(step);
        }

        public void ClearSteps()
        {
            _steps.Clear();
        }

        public void RemoveStep(IWorkflowStepSettings step)
        {
            _steps.Remove(step);
        }


    }
}
