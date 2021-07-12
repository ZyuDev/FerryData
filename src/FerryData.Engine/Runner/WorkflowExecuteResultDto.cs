using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Runner
{
    public class WorkflowExecuteResultDto
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public List<WorkflowStepExecuteResultDto> StepResults { get; set; } = new List<WorkflowStepExecuteResultDto>();
        public IEnumerable<string> LogMessages { get; set; } = new List<string>();

    }
}
