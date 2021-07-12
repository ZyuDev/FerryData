using FerryData.Engine.Abstract;
using FerryData.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Runner
{
    public class WorkflowRunner
    {
        private readonly IWorkflowSettings _settings;

        public Workflow Workflow { get; private set; }

        public WorkflowRunner(IWorkflowSettings settings)
        {
            _settings = settings;

            Workflow = new Workflow();
            Workflow.Uid = settings.Uid;

            Workflow.Settings = settings;

            foreach (var settingsStep in Workflow.Settings.Steps)
            {
                var newStep = new WorkflowStep();
                newStep.Uid = settingsStep.Uid;
                newStep.Settings = settingsStep;

                Workflow.AddStep(newStep);
            }
        }

    }
}
