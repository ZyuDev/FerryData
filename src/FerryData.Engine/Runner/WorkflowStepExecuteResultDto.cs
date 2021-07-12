using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Runner
{
    public class WorkflowStepExecuteResultDto
    {
        public Guid Uid { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public bool Finished { get; set; }
    }
}
