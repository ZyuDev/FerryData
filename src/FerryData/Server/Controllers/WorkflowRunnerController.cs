using FerryData.Engine.Abstract;
using FerryData.Engine.Models;
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
        public ResponseDto<int> Execute(WorkflowSettings settingsItem)
        {
            var responseDto = new ResponseDto<int>();

            var item = _service.GetItem(settingsItem.Uid);

            if (item == null)
            {
                responseDto.Status = -1;
                responseDto.Message = $"Settings not find {settingsItem.Uid}";
            }
           

            return responseDto;
        }

    }
}
