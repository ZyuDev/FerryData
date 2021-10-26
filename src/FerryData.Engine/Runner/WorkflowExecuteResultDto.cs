using System.Collections.Generic;
using System.Linq;

namespace FerryData.Engine.Runner
{
    public class WorkflowExecuteResultDto
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public List<WorkflowStepExecuteResultDto> StepResults { get; set; } = new List<WorkflowStepExecuteResultDto>();
        public List<string> LogMessages { get; set; } = new List<string>();

        public bool AllStepsDone()
        {
            return StepResults.All(x => x.Finished);
        }

    }
}
