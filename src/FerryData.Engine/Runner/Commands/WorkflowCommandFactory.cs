using FerryData.Engine.Abstract;
using FerryData.Engine.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Runner.Commands
{
    public class WorkflowCommandFactory
    {
        public static IWorkflowCommand Create(IWorkflowStepAction actionSettings, Dictionary<string, object> stepsData, Logger logger)
        {
            IWorkflowCommand command = null;
            switch (actionSettings.Kind)
            {
                case Enums.WorkflowActionKinds.Sleep:

                    command = new WorkflowSleepCommand((WorkflowSleepAction)actionSettings, logger);
                    break;

                case Enums.WorkflowActionKinds.HttpConnector:

                    var httpAction = (WorkflowHttpAction)actionSettings;

                    if (httpAction.SendToRabbit)
                    {
                        command = new WorkflowSendToRabbitCommand(httpAction, stepsData, logger);
                    }
                    else
                    {
                        command = new WorkflowHttpCommand(httpAction, stepsData, logger);
                    }
                    break;

                default:
                    break;
            }

            return command;
        }
    }
}
