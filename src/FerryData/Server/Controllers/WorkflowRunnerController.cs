using FerryData.Engine.Abstract;
using FerryData.Engine.Models;
using FerryData.Engine.Runner;
using FerryData.Server.Services;
using FerryData.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryData.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkflowRunnerController : ControllerBase
    {
        private IWorkflowSettingsService _service;

        public WorkflowRunnerController(IWorkflowSettingsService service)
        {
            _service = service;
        }

        [HttpPost("Execute")]
        public async Task<WorkflowExecuteResultDto> Execute(WorkflowSettings settingsItem)
        {
            var executeResult = new WorkflowExecuteResultDto();

            var item = _service.GetItem(settingsItem.Uid);

            if (item == null)
            {
                executeResult.Status = -1;
                executeResult.Message = $"Settings not find {settingsItem.Uid}";
            }
            else
            {
                var runner = new WorkflowRunner(item);

                await runner.Run();

                executeResult.LogMessages = runner.Messages;
            }
           

            return executeResult;
        }

    }
}
