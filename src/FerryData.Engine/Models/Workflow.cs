using FerryData.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Models
{
    public class Workflow : IWorkflow
    {

        private List<IWorkflowStep> _steps = new List<IWorkflowStep>();

        public Guid Uid { get; set; }
        public IWorkflowSettings Settings { get; set; }

        public IEnumerable<IWorkflowStep> Steps => _steps;

        public bool Finished { get; set; }

        public IWorkflowStep GetStartStep()
        {
            var step = _steps.FirstOrDefault(x => !x.Finished && x.Settings.InUid == Guid.Empty);

            return step;
        }

        public IWorkflowStep GetNextStep(IWorkflowStep step)
        {
            var nextStep = _steps.FirstOrDefault(x => !x.Finished && x.Settings.InUid == step.Uid);

            return nextStep;
        }

        public void AddStep(IWorkflowStep step)
        {
            _steps.Add(step);
        }
    }
}
