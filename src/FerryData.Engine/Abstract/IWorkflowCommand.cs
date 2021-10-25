using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine.Abstract
{
    public interface IWorkflowCommand
    {
        Task<IWorkflowCommandResult> ExecuteAsync();
    }
}
