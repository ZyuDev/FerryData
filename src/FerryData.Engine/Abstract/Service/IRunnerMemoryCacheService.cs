using FerryData.Engine.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Abstract.Service
{
    public interface IRunnerMemoryCacheService
    {
        void SetResult(WorkflowExecuteResultDto resultDto);
        WorkflowExecuteResultDto GetResult();
    }
}
