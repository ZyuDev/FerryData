using FerryData.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FerryData.ActiveObjectsClassLibrary;

namespace FerryData.Engine.Models
{

    public class WorkflowStep : ActiveObject
    {
        public WorkflowStep(Scenario _scenario, bool _imReadingMail = true) : base(_scenario, _imReadingMail)
        {

        }

        public Guid Uid { get; set; }
        public bool Finished { get; set; }
        public object Data { get; set; }
        public IWorkflowStepSettings Settings { get; set; }

        public override string objectType => throw new NotImplementedException();

        public override void processIncomingMessage(IInterObjectMessage msg)
        {
            
        }
    }
}
