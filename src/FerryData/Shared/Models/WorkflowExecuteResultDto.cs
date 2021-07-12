using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Shared.Models
{
    public class WorkflowExecuteResultDto
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public IEnumerable<string> LogMessages { get; set; } = new List<string>();
    }
}
