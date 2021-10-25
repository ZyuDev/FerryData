using FerryData.Engine.Abstract;

namespace FerryData.Engine.Runner
{
    public class WorkflowStepExecuteResult: IWorkflowCommandResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

    }
}