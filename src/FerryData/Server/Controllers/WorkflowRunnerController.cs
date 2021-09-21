using FerryData.Engine.Runner;
using FerryData.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FerryData.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class WorkflowRunnerController : ControllerBase
    {
        private readonly IWorkflowSettingsServiceAsync _dbService;

        public WorkflowRunnerController(IWorkflowSettingsServiceAsync db)
        {
            _dbService = db;
        }

        [HttpGet("Execute/{Uid}")]
        public async Task<WorkflowExecuteResultDto> Execute(Guid Uid)
        {
            var executeResult = new WorkflowExecuteResultDto();

            var item = await _dbService.GetItem(Uid);

            if (item == null)
            {
                executeResult.Status = -1;
                executeResult.Message = $"Settings not find {Uid}";
            }
            else
            {
                var runner = new WorkflowRunner(item);

                await runner.Run();

                executeResult.LogMessages = runner.Messages;

                foreach (var step in runner.Workflow.Steps)
                {
                    var stepDto = new WorkflowStepExecuteResultDto()
                    {
                        Uid = step.Uid,
                        Title = step.Settings.Title,
                        Finished = step.Finished,
                    };

                    if (step.Data != null)
                    {
                        stepDto.Data = JsonConvert.SerializeObject(step.Data);
                    }
                    executeResult.StepResults.Add(stepDto);
                }

            }


            return executeResult;
        }

    }
}
